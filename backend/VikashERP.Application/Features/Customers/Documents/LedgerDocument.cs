using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VikashERP.Domain.Entities;

namespace VikashERP.Application.Features.Customers.Documents;

public class LedgerDocument : IDocument
{
    private readonly Customer _customer;
    private readonly List<CustomerLedger> _entries;
    private readonly DateTime? _fromDate;
    private readonly DateTime? _toDate;

    public LedgerDocument(Customer customer, List<CustomerLedger> entries, DateTime? fromDate, DateTime? toDate)
    {
        _customer = customer;
        _entries = entries;
        _fromDate = fromDate;
        _toDate = toDate;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(15, Unit.Millimetre);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
    }

    void ComposeHeader(IContainer container)
    {
        container.PaddingBottom(10).Column(column =>
        {
            column.Item().AlignCenter().Text("VIKASH IRON & STEEL").FontSize(16).Bold();
            column.Item().AlignCenter().Text("CUSTOMER LEDGER STATEMENT").FontSize(12).Bold().Underline();
            
            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text(t =>
                    {
                        t.Span("Customer Name: ").Bold();
                        t.Span(_customer.CompanyName ?? $"{_customer.FirstName} {_customer.LastName}");
                    });
                    if (!string.IsNullOrEmpty(_customer.Phone))
                    {
                        c.Item().Text(t => { t.Span("Phone: ").Bold(); t.Span(_customer.Phone); });
                    }
                    if (!string.IsNullOrEmpty(_customer.Address))
                    {
                        c.Item().Text(t => { t.Span("Address: ").Bold(); t.Span(_customer.Address); });
                    }
                });

                row.RelativeItem().AlignRight().Column(c =>
                {
                    if (_fromDate.HasValue || _toDate.HasValue)
                    {
                        string dateRange = "";
                        if (_fromDate.HasValue && _toDate.HasValue)
                            dateRange = $"{_fromDate.Value:dd-MMM-yyyy} to {_toDate.Value:dd-MMM-yyyy}";
                        else if (_fromDate.HasValue)
                            dateRange = $"From {_fromDate.Value:dd-MMM-yyyy}";
                        else if (_toDate.HasValue)
                            dateRange = $"Up to {_toDate.Value:dd-MMM-yyyy}";
                            
                        c.Item().Text(t => { t.Span("Period: ").Bold(); t.Span(dateRange); });
                    }
                    c.Item().Text(t => { t.Span("Generated On: ").Bold(); t.Span(DateTime.Now.ToString("dd-MMM-yyyy HH:mm")); });
                });
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(80);  // Date
                columns.RelativeColumn(3);   // Particulars/Remarks
                columns.RelativeColumn(1);   // Debit
                columns.RelativeColumn(1);   // Credit
                columns.RelativeColumn(1);   // Balance
            });

            table.Header(header =>
            {
                header.Cell().BorderBottom(1).PaddingBottom(5).Text("Date").Bold();
                header.Cell().BorderBottom(1).PaddingBottom(5).Text("Particulars").Bold();
                header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Debit (Rs)").Bold();
                header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Credit (Rs)").Bold();
                header.Cell().BorderBottom(1).PaddingBottom(5).AlignRight().Text("Balance (Rs)").Bold();
            });

            decimal totalDebit = 0;
            decimal totalCredit = 0;

            foreach (var entry in _entries)
            {
                totalDebit += entry.Debit;
                totalCredit += entry.Credit;

                table.Cell().PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Text(entry.TransactionDate.ToString("dd-MMM-yyyy"));
                table.Cell().PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Text(entry.Remarks ?? entry.TransactionType);
                table.Cell().PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).AlignRight().Text(entry.Debit > 0 ? entry.Debit.ToString("N2") : "-");
                table.Cell().PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).AlignRight().Text(entry.Credit > 0 ? entry.Credit.ToString("N2") : "-");
                
                string balanceStr = entry.RunningBalance >= 0 
                    ? $"{entry.RunningBalance:N2} Dr" 
                    : $"{Math.Abs(entry.RunningBalance):N2} Cr";
                    
                table.Cell().PaddingVertical(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).AlignRight().Text(balanceStr);
            }

            // Totals Row
            table.Cell().ColumnSpan(2).PaddingTop(5).AlignRight().Text("Total:").Bold();
            table.Cell().PaddingTop(5).AlignRight().Text(totalDebit.ToString("N2")).Bold();
            table.Cell().PaddingTop(5).AlignRight().Text(totalCredit.ToString("N2")).Bold();
            table.Cell().PaddingTop(5).Text("");
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.Span("Page ");
            x.CurrentPageNumber();
            x.Span(" of ");
            x.TotalPages();
        });
    }
}
