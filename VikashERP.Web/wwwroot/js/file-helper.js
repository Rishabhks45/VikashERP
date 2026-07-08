window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
};

window.printPdfBase64 = (pdfBase64Url) => {
    var printWindow = window.open("", "_blank");
    printWindow.document.write('<html><head><title>Print PDF</title></head><body style="margin:0;padding:0;"><embed src="' + pdfBase64Url + '" type="application/pdf" width="100%" height="100%" id="pdfEmbed"></embed></body></html>');
    printWindow.document.close();
    setTimeout(function() {
        printWindow.print();
    }, 500);
};
