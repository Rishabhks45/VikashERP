using System;
using MudBlazor;

namespace VikashERP.Web.Models;

public record Metric(string Label, string Value, string Trend, string Icon, string IconClass, string TrendClass);

// Executive Dashboard (Home.razor)
public record RevenuePoint(string Day, int Sales, int Collected);
public record DispatchItem(string Vehicle, string Customer, string Status, string StatusClass);
public record StockItem(string Material, string Location, int Percent, string Quantity);
public record InvoiceItem(string Number, string Customer, string Amount, string Status, string StatusClass);

// Employee Dashboard
public record WorkProgressPoint(string Day, int Planned, int Completed);
public record AssignmentItem(string Title, string Detail, string Status, string StatusClass);
public record ShiftItem(string Label, string Time, int Progress, string Status);
public record RequestItem(string Title, string Category, string Date, string Status, string StatusClass);

// Customer Dashboard
public record OrderRecord(string OrderNo, DateTime Date, string Material, decimal Amount, string Status, Color StatusColor);
