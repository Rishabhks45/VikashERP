using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using Microsoft.Maui.Storage;
using VikashERP.Mobile.Services.Interfaces;

namespace VikashERP.Mobile.Services;

public class PdfExportService : IPdfExportService
{
    public async Task<(bool IsSuccess, string ErrorMessage)> SaveAndSharePdfAsync(string base64Content, string filename)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(base64Content))
                return (false, "PDF content is empty.");

            byte[] pdfBytes = Convert.FromBase64String(base64Content);
            
            // Save to cache directory
            string cacheDir = FileSystem.CacheDirectory;
            string filePath = Path.Combine(cacheDir, filename);
            
            await File.WriteAllBytesAsync(filePath, pdfBytes);
            
            // Execute on Main Thread to prevent cross-thread issues
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Share Invoice",
                    File = new ShareFile(filePath)
                });
            });
            
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error exporting PDF: {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> SaveAndOpenPdfAsync(string base64Content, string filename)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(base64Content))
                return (false, "PDF content is empty.");

            byte[] pdfBytes = Convert.FromBase64String(base64Content);
            
            string downloadPath;
            
#if WINDOWS
            downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", filename);
#elif ANDROID
            downloadPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, filename);
#else
            downloadPath = Path.Combine(FileSystem.CacheDirectory, filename);
#endif
            
            await File.WriteAllBytesAsync(downloadPath, pdfBytes);
            
#if ANDROID
            try
            {
                // Add to Android's DownloadManager to show a system notification!
                var downloadManager = (Android.App.DownloadManager?)Platform.AppContext.GetSystemService(Android.Content.Context.DownloadService);
                if (downloadManager != null)
                {
                    downloadManager.AddCompletedDownload(
                        filename,
                        "Invoice downloaded successfully",
                        true, // isMediaScannerScannable
                        "application/pdf",
                        downloadPath,
                        pdfBytes.Length,
                        true // showNotification
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to notify DownloadManager: {ex.Message}");
            }
#endif
            
            return (true, "PDF saved successfully!");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving PDF: {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> PrintPdfAsync(string base64String, string filename)
    {
        try
        {
            var pdfBytes = Convert.FromBase64String(base64String);

#if ANDROID
            var activity = Platform.CurrentActivity;
            if (activity == null) return (false, "Current Activity not found");
            
            var printManager = (Android.Print.PrintManager?)activity.GetSystemService(Android.Content.Context.PrintService);
            
            if (printManager != null)
            {
                var printAdapter = new PdfDocumentAdapter(filename, pdfBytes);
                printManager.Print(filename, printAdapter, new Android.Print.PrintAttributes.Builder()?.Build());
                return (true, "Print dialog opened!");
            }
            return (false, "Print service not found on device.");
#else
            // Fallback for non-Android platforms (just open the PDF)
            return await SaveAndOpenPdfAsync(base64String, filename);
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error printing PDF: {ex.Message}");
            return (false, ex.Message);
        }
    }
}

#if ANDROID
public class PdfDocumentAdapter : Android.Print.PrintDocumentAdapter
{
    private readonly string _fileName;
    private readonly byte[] _pdfBytes;

    public PdfDocumentAdapter(string fileName, byte[] pdfBytes)
    {
        _fileName = fileName;
        _pdfBytes = pdfBytes;
    }

    public override void OnLayout(Android.Print.PrintAttributes? oldAttributes, Android.Print.PrintAttributes? newAttributes, Android.OS.CancellationSignal? cancellationSignal, LayoutResultCallback? callback, Android.OS.Bundle? extras)
    {
        if (cancellationSignal?.IsCanceled == true)
        {
            callback?.OnLayoutCancelled();
            return;
        }

        var info = new Android.Print.PrintDocumentInfo.Builder(_fileName)
            ?.SetContentType(Android.Print.PrintContentType.Document)
            ?.SetPageCount(Android.Print.PrintDocumentInfo.PageCountUnknown)
            ?.Build();

        callback?.OnLayoutFinished(info, !oldAttributes?.Equals(newAttributes) ?? true);
    }

    public override void OnWrite(Android.Print.PageRange[]? pages, Android.OS.ParcelFileDescriptor? destination, Android.OS.CancellationSignal? cancellationSignal, WriteResultCallback? callback)
    {
        if (destination == null || _pdfBytes == null || _pdfBytes.Length == 0)
        {
            callback?.OnWriteFailed("Invalid destination or empty PDF");
            return;
        }

        try
        {
            using (var outputStream = new Java.IO.FileOutputStream(destination.FileDescriptor))
            {
                outputStream.Write(_pdfBytes);
            }
            callback?.OnWriteFinished(new Android.Print.PageRange[] { Android.Print.PageRange.AllPages });
        }
        catch (Exception ex)
        {
            callback?.OnWriteFailed(ex.Message);
        }
    }
}
#endif
