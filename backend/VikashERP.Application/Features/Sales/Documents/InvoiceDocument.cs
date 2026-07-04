using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VikashERP.Application.Features.Sales.DTOs;
using System.Linq;
using System;

namespace VikashERP.Application.Features.Sales.Documents;

public class InvoiceDocument : IDocument
{
    private readonly InvoiceDetailDto _invoice;
    private readonly dynamic _customer;
    private readonly bool _isGstBill;

    public InvoiceDocument(InvoiceDetailDto invoice, dynamic customer)
    {
        _invoice = invoice;
        _customer = customer;
        _isGstBill = _invoice.CgstAmount > 0 || _invoice.SgstAmount > 0 || _invoice.IgstAmount > 0;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(10, Unit.Millimetre);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Arial));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
    }

    void ComposeHeader(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Text(_isGstBill ? "TAX INVOICE" : "INVOICE").FontSize(14).Bold();
            column.Item().PaddingTop(5).Text(text =>
            {
                text.Span("IRN: -\n").FontSize(8);
                text.Span("Ack No.: -\n").FontSize(8);
                text.Span("Ack Date: -").FontSize(8);
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingTop(10).Column(column =>
        {
            column.Item().Border(1).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                // Row 1
                table.Cell().RowSpan(2).BorderRight(1).BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("VIKASH IRON & STEEL").FontSize(12).Bold();
                    c.Item().Text("Reg Office :- ").FontSize(8); // Address can be passed in if needed
                    c.Item().Text("GSTIN/UIN: 10CTHPS4609D2ZY").FontSize(8);
                    c.Item().Text("State Name : Bihar, Code : 10").FontSize(8);
                    c.Item().Text("E-Mail : ").FontSize(8);
                });
                table.Cell().BorderRight(1).BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("Invoice No.").FontSize(8);
                    c.Item().Text(_invoice.InvoiceNumber).Bold();
                });
                table.Cell().BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("Dated").FontSize(8);
                    c.Item().Text(_invoice.InvoiceDate.ToString("dd-MMM-yy")).Bold();
                });

                // Row 2
                table.Cell().BorderRight(1).BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("Delivery Note").FontSize(8);
                    c.Item().Text("E-WAY BILL").Bold();
                });
                table.Cell().BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("Mode/Terms of Payment").FontSize(8);
                    c.Item().Text(_invoice.PaymentMode).Bold();
                });

                // Row 3 (Buyer Details)
                string customerName = _customer != null ? (string)(_customer.CompanyName ?? $"{_customer.FirstName} {_customer.LastName}") : "";
                string customerAddress = _customer != null && _customer.Address != null ? (string)_customer.Address : "";
                string customerGstin = _customer != null && _customer.Gstin != null ? (string)_customer.Gstin : "";
                string stateCode = !string.IsNullOrEmpty(customerGstin) && customerGstin.Length >= 2 ? customerGstin.Substring(0, 2) : "10";

                table.Cell().BorderRight(1).BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("Buyer (Bill to)").FontSize(8);
                    c.Item().Text(customerName).FontSize(11).Bold();
                    c.Item().Text(customerAddress).FontSize(8);
                    if (_isGstBill)
                    {
                        c.Item().Text($"GSTIN/UIN : {customerGstin}").FontSize(8);
                        c.Item().Text($"State Name : Bihar, Code : {stateCode}").FontSize(8);
                        c.Item().Text("Place of Supply : Bihar").FontSize(8);
                    }
                });
                table.Cell().BorderRight(1).BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("Dispatched through").FontSize(8);
                    c.Item().Text("BY ROAD").Bold();
                });
                table.Cell().BorderBottom(1).Padding(5).Column(c =>
                {
                    c.Item().Text("Motor Vehicle No.").FontSize(8);
                    c.Item().Text(_invoice.VehicleNumber ?? "").Bold();
                });
            });

            column.Item().Element(ComposeItemsTable);
            
            if (_isGstBill)
            {
                column.Item().Element(ComposeTaxBreakdownTable);
            }
        });
    }

    void ComposeItemsTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25); // Sl No
                columns.RelativeColumn(3);  // Description
                if (_isGstBill)
                {
                    columns.RelativeColumn(1);  // HSN/SAC
                }
                columns.RelativeColumn(1);  // Quantity
                columns.RelativeColumn(1);  // Rate
                columns.ConstantColumn(30); // per
                columns.RelativeColumn(1);  // Amount
            });

            table.Header(header =>
            {
                header.Cell().BorderLeft(1).BorderRight(1).BorderBottom(1).Padding(3).AlignCenter().Text("Sl\nNo.");
                header.Cell().BorderRight(1).BorderBottom(1).Padding(3).AlignLeft().Text("Description of Goods and Services");
                if (_isGstBill)
                {
                    header.Cell().BorderRight(1).BorderBottom(1).Padding(3).AlignCenter().Text("HSN/SAC");
                }
                header.Cell().BorderRight(1).BorderBottom(1).Padding(3).AlignRight().Text("Quantity");
                header.Cell().BorderRight(1).BorderBottom(1).Padding(3).AlignRight().Text("Rate");
                header.Cell().BorderRight(1).BorderBottom(1).Padding(3).AlignCenter().Text("per");
                header.Cell().BorderRight(1).BorderBottom(1).Padding(3).AlignRight().Text("Amount");
            });

            int slNo = 1;
            decimal totalQty = 0;
            decimal totalTaxable = 0;

            foreach (var item in _invoice.Items)
            {
                totalQty += item.WeightKg;
                totalTaxable += item.TotalPrice;

                table.Cell().BorderLeft(1).BorderRight(1).Padding(3).AlignCenter().Text(slNo.ToString());
                table.Cell().BorderRight(1).Padding(3).Text($"{item.CategoryName} {item.VariantName}").Bold();
                if (_isGstBill)
                {
                    table.Cell().BorderRight(1).Padding(3).AlignCenter().Text(item.HsnCode);
                }
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{item.WeightKg:0.000} {item.RateOn}").Bold();
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{item.Rate:0.00}");
                table.Cell().BorderRight(1).Padding(3).AlignCenter().Text(item.RateOn);
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{item.TotalPrice:0.00}").Bold();
                
                slNo++;
            }

            // Filler
            table.Cell().BorderLeft(1).BorderRight(1).Padding(3).MinHeight(100).Text("");
            table.Cell().BorderRight(1).Padding(3).Text("");
            if (_isGstBill) table.Cell().BorderRight(1).Padding(3).Text("");
            table.Cell().BorderRight(1).Padding(3).Text("");
            table.Cell().BorderRight(1).Padding(3).Text("");
            table.Cell().BorderRight(1).Padding(3).Text("");
            table.Cell().BorderRight(1).Padding(3).AlignBottom().AlignRight().Text($"{totalTaxable:N2}").Bold();

            // Charges and Taxes
            if (_invoice.LoadingCharge > 0)
            {
                table.Cell().BorderLeft(1).BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text("Loading Charges (With Tax)").Bold();
                if (_isGstBill) table.Cell().BorderRight(1).Padding(3).AlignCenter().Text("996511");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{_invoice.LoadingCharge:N2}").Bold();
            }
            if (_invoice.FreightCharge > 0)
            {
                table.Cell().BorderLeft(1).BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text("Freight Charges").Bold();
                if (_isGstBill) table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{_invoice.FreightCharge:N2}").Bold();
            }

            if (_isGstBill)
            {
                if (_invoice.CgstAmount > 0)
                {
                    table.Cell().BorderLeft(1).BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).AlignRight().Text("CGST").Bold();
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{_invoice.CgstAmount:N2}").Bold();
                }
                if (_invoice.SgstAmount > 0)
                {
                    table.Cell().BorderLeft(1).BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).AlignRight().Text("SGST").Bold();
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{_invoice.SgstAmount:N2}").Bold();
                }
                if (_invoice.IgstAmount > 0)
                {
                    table.Cell().BorderLeft(1).BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).AlignRight().Text("IGST").Bold();
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).Text("");
                    table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{_invoice.IgstAmount:N2}").Bold();
                }
            }

            if (_invoice.RoundingAmount != 0)
            {
                table.Cell().BorderLeft(1).BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text("R/off").Bold();
                if (_isGstBill) table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).Text("");
                table.Cell().BorderRight(1).Padding(3).AlignRight().Text($"{_invoice.RoundingAmount:N2}").Bold();
            }

            // Footer
            var colspan = _isGstBill ? 3u : 2u;
            table.Cell().ColumnSpan(colspan).BorderLeft(1).BorderRight(1).BorderTop(1).BorderBottom(1).Padding(3).AlignRight().Text("Total").Bold();
            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(3).AlignRight().Text($"{totalQty:0.000} Kg").Bold();
            table.Cell().ColumnSpan(2).BorderRight(1).BorderTop(1).BorderBottom(1).Padding(3).Text("");
            table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(3).AlignRight().Text($"Rs {_invoice.TotalAmount:N2}").Bold();
        });

        container.BorderLeft(1).BorderRight(1).BorderBottom(1).Padding(5).Row(row =>
        {
            row.RelativeItem().Column(c =>
            {
                c.Item().Text("Amount Chargeable (in words)").FontSize(8);
                c.Item().Text($"INR {_invoice.TotalAmount:N2}").Bold();
            });
            row.ConstantItem(50).AlignRight().Text("E. & O.E").FontSize(8);
        });
    }

    void ComposeTaxBreakdownTable(IContainer container)
    {
        decimal totalHsnTax = 0;
        
        container.Column(col => 
        {
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().RowSpan(2).BorderLeft(1).BorderRight(1).BorderBottom(1).AlignCenter().AlignMiddle().Text("HSN/SAC");
                    header.Cell().RowSpan(2).BorderRight(1).BorderBottom(1).AlignCenter().AlignMiddle().Text("Taxable\nValue");
                    header.Cell().ColumnSpan(2).BorderRight(1).BorderBottom(1).AlignCenter().Text("CGST");
                    header.Cell().ColumnSpan(2).BorderRight(1).BorderBottom(1).AlignCenter().Text("SGST/UTGST");
                    header.Cell().RowSpan(2).BorderRight(1).BorderBottom(1).AlignCenter().AlignMiddle().Text("Total\nTax Amount");

                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("Rate");
                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("Amount");
                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("Rate");
                    header.Cell().BorderRight(1).BorderBottom(1).AlignCenter().Text("Amount");
                });

                var hsnGroups = _invoice.Items.GroupBy(i => i.HsnCode).ToList();
                decimal totalHsnTaxable = 0;
                decimal totalHsnCgst = 0;
                decimal totalHsnSgst = 0;

                foreach (var group in hsnGroups)
                {
                    var hsnTaxable = group.Sum(i => i.TotalPrice);
                    var hsnCgst = hsnTaxable * (group.First().CgstRate / 100m);
                    var hsnSgst = hsnTaxable * (group.First().SgstRate / 100m);
                    var hsnTotalTax = hsnCgst + hsnSgst;
                    
                    totalHsnTaxable += hsnTaxable;
                    totalHsnCgst += hsnCgst;
                    totalHsnSgst += hsnSgst;
                    totalHsnTax += hsnTotalTax;

                    table.Cell().BorderLeft(1).BorderRight(1).Padding(2).AlignCenter().Text(group.Key);
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{hsnTaxable:N2}");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{group.First().CgstRate:0}%");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{hsnCgst:N2}");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{group.First().SgstRate:0}%");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{hsnSgst:N2}");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{hsnTotalTax:N2}");
                }
                
                if (_invoice.LoadingCharge > 0)
                {
                    var loadingCgst = _invoice.LoadingCharge * 0.09m;
                    var loadingSgst = _invoice.LoadingCharge * 0.09m;
                    totalHsnTaxable += _invoice.LoadingCharge;
                    totalHsnCgst += loadingCgst;
                    totalHsnSgst += loadingSgst;
                    totalHsnTax += (loadingCgst + loadingSgst);

                    table.Cell().BorderLeft(1).BorderRight(1).Padding(2).AlignCenter().Text("996511");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{_invoice.LoadingCharge:N2}");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text("9%");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{loadingCgst:N2}");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text("9%");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{loadingSgst:N2}");
                    table.Cell().BorderRight(1).Padding(2).AlignRight().Text($"{loadingCgst + loadingSgst:N2}");
                }

                table.Cell().BorderLeft(1).BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignRight().Text("Total").Bold();
                table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignRight().Text($"{totalHsnTaxable:N2}").Bold();
                table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).Text("");
                table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignRight().Text($"{totalHsnCgst:N2}").Bold();
                table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).Text("");
                table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignRight().Text($"{totalHsnSgst:N2}").Bold();
                table.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).Padding(2).AlignRight().Text($"{totalHsnTax:N2}").Bold();
            });

            col.Item().BorderLeft(1).BorderRight(1).BorderBottom(1).Padding(5).Row(row =>
            {
                row.RelativeItem().Text(t =>
                {
                    t.Span("Tax Amount (in words) : ").FontSize(8);
                    t.Span($"INR {totalHsnTax:N2}").Bold();
                });
            });
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.BorderLeft(1).BorderRight(1).BorderBottom(1).Row(row =>
        {
            row.RelativeItem().Padding(5).Column(c =>
            {
                c.Item().Text("Declaration").FontSize(8).Underline();
                c.Item().Text("We declare that this invoice shows the actual price of the\ngoods described and that all particulars are true and").FontSize(8);
            });
            row.RelativeItem().BorderLeft(1).Padding(5).AlignRight().Column(c =>
            {
                c.Item().Text("for VIKASH IRON & STEEL").Bold();
                c.Item().Height(40);
                c.Item().Text("Authorised Signatory").FontSize(8);
            });
        });
        
        container.Padding(5).Column(c =>
        {
            c.Item().AlignCenter().Text("SUBJECT TO PATNA JURISDICTION").FontSize(8);
            c.Item().AlignCenter().Text("This is a Computer Generated Invoice").FontSize(8);
        });
    }
}
