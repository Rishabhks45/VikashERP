-- PostgreSQL Database Schema for Vikash Iron & Steel ERP
-- Core Modules: Customer Ledger, Inventory, Billing, Deliveries, and Staff Management.

-- ============================================================================
-- 1. MASTER MODULE & USER CONFIGURATION
-- ============================================================================

CREATE TABLE godowns (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    location VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE suppliers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    company_name VARCHAR(255),
    phone VARCHAR(20) NOT NULL,
    gstin VARCHAR(15),
    address TEXT,
    current_balance DECIMAL(12, 2) DEFAULT 0.00, -- Negative represents dues, positive represents advance
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    account_number VARCHAR(20) NOT NULL UNIQUE,  -- Auto-generated AC No, e.g. CUS-2026-000001
    name VARCHAR(255) NOT NULL,
    company_name VARCHAR(255),
    phone VARCHAR(20) NOT NULL,
    email VARCHAR(255),
    gstin VARCHAR(15),
    address TEXT,
    credit_limit DECIMAL(12, 2) DEFAULT 0.00,
    current_balance DECIMAL(12, 2) DEFAULT 0.00, -- Negative represents dues (customer owes us), positive represents advance
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE UNIQUE INDEX idx_customers_account_number ON customers (account_number);

-- ============================================================================
-- 2. PRODUCT CONFIGURATION (Multi-level Variant Structure)
-- ============================================================================

CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    category_id INT REFERENCES categories(id) ON DELETE RESTRICT,
    name VARCHAR(255) NOT NULL,
    hsn_code VARCHAR(10),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE product_variants (
    id SERIAL PRIMARY KEY,
    product_id INT REFERENCES products(id) ON DELETE CASCADE,
    size VARCHAR(50) NOT NULL,        -- e.g., '1 inch', '2 inch'
    thickness VARCHAR(50) NOT NULL,   -- e.g., '2mm', '3mm'
    unit_pcs_to_kg DECIMAL(12, 4) DEFAULT 1.0000, -- Piece to KG weight conversion ratio
    alert_qty_pcs INT DEFAULT 10,     -- Minimum stock threshold
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_product_variant UNIQUE (product_id, size, thickness)
);

-- ============================================================================
-- 3. INVENTORY & STOCK LEDGER
-- ============================================================================

CREATE TABLE stock_ledger (
    id SERIAL PRIMARY KEY,
    variant_id INT REFERENCES product_variants(id) ON DELETE RESTRICT,
    godown_id INT REFERENCES godowns(id) ON DELETE RESTRICT,
    transaction_type VARCHAR(50) NOT NULL, -- 'PURCHASE', 'SALE', 'TRANSFER_IN', 'TRANSFER_OUT', 'ADJUSTMENT', 'SCRAP', 'CUTTING_LOSS'
    reference_id INT, -- ID of Invoice, Purchase, or Transfer records
    qty_pcs INT DEFAULT 0, -- Positive for addition, negative for deduction
    weight_kg DECIMAL(12, 3) DEFAULT 0.000,
    running_pcs INT NOT NULL,
    running_weight_kg DECIMAL(12, 3) NOT NULL,
    remarks TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================================================
-- 4. SALES, BILLING & INVOICES
-- ============================================================================

CREATE TABLE invoices (
    id SERIAL PRIMARY KEY,
    invoice_number VARCHAR(100) NOT NULL UNIQUE,
    customer_id INT REFERENCES customers(id) ON DELETE RESTRICT,
    subtotal DECIMAL(12, 2) NOT NULL,
    cgst_amount DECIMAL(12, 2) DEFAULT 0.00,
    sgst_amount DECIMAL(12, 2) DEFAULT 0.00,
    igst_amount DECIMAL(12, 2) DEFAULT 0.00,
    total_amount DECIMAL(12, 2) NOT NULL,
    paid_amount DECIMAL(12, 2) DEFAULT 0.00,
    due_amount DECIMAL(12, 2) NOT NULL,
    payment_mode VARCHAR(50) NOT NULL, -- 'CASH', 'UPI', 'NEFT', 'CREDIT', 'MIXED'
    invoice_date DATE NOT NULL DEFAULT CURRENT_DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE invoice_items (
    id SERIAL PRIMARY KEY,
    invoice_id INT REFERENCES invoices(id) ON DELETE CASCADE,
    variant_id INT REFERENCES product_variants(id) ON DELETE RESTRICT,
    qty_pcs INT NOT NULL,
    weight_kg DECIMAL(12, 3) NOT NULL,
    rate_per_kg DECIMAL(12, 2) NOT NULL,
    cgst_rate DECIMAL(5, 2) DEFAULT 9.00,
    sgst_rate DECIMAL(5, 2) DEFAULT 9.00,
    igst_rate DECIMAL(5, 2) DEFAULT 0.00,
    total_price DECIMAL(12, 2) NOT NULL
);

-- ============================================================================
-- 5. ACCOUNTS & FINANCIAL LEDGER
-- ============================================================================

CREATE TABLE customer_ledger (
    id SERIAL PRIMARY KEY,
    customer_id INT REFERENCES customers(id) ON DELETE CASCADE,
    transaction_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    transaction_type VARCHAR(50) NOT NULL, -- 'INVOICE', 'PAYMENT', 'RETURN', 'ADJUSTMENT'
    reference_id INT, -- ID of the Invoice or Receipt
    debit DECIMAL(12, 2) DEFAULT 0.00, -- Customer owes us more
    credit DECIMAL(12, 2) DEFAULT 0.00, -- Customer paid us
    running_balance DECIMAL(12, 2) NOT NULL,
    remarks TEXT
);

CREATE TABLE supplier_ledger (
    id SERIAL PRIMARY KEY,
    supplier_id INT REFERENCES suppliers(id) ON DELETE CASCADE,
    transaction_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    transaction_type VARCHAR(50) NOT NULL, -- 'PURCHASE', 'PAYMENT', 'RETURN', 'ADJUSTMENT'
    reference_id INT,
    debit DECIMAL(12, 2) DEFAULT 0.00, -- We paid supplier
    credit DECIMAL(12, 2) DEFAULT 0.00, -- We purchased from supplier
    running_balance DECIMAL(12, 2) NOT NULL,
    remarks TEXT
);

-- ============================================================================
-- 6. DISPATCH & DELIVERY MODULE
-- ============================================================================

CREATE TABLE deliveries (
    id SERIAL PRIMARY KEY,
    invoice_id INT REFERENCES invoices(id) ON DELETE CASCADE,
    vehicle_number VARCHAR(20) NOT NULL,
    driver_name VARCHAR(100) NOT NULL,
    driver_phone VARCHAR(20),
    delivery_status VARCHAR(50) DEFAULT 'PENDING', -- 'PENDING', 'LOADED', 'IN_TRANSIT', 'DELIVERED', 'CANCELLED'
    delivery_challan_number VARCHAR(100) UNIQUE,
    loading_charge DECIMAL(10, 2) DEFAULT 0.00,
    freight_charge DECIMAL(10, 2) DEFAULT 0.00,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================================================
-- 7. STAFF MANAGEMENT
-- ============================================================================

CREATE TABLE staff (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    role VARCHAR(50) NOT NULL, -- 'ADMIN', 'SALES', 'INVENTORY', 'DRIVER', 'LOADER'
    salary_per_month DECIMAL(10, 2) NOT NULL,
    phone VARCHAR(20) NOT NULL UNIQUE,
    hire_date DATE DEFAULT CURRENT_DATE
);

CREATE TABLE attendance (
    id SERIAL PRIMARY KEY,
    staff_id INT REFERENCES staff(id) ON DELETE CASCADE,
    work_date DATE NOT NULL DEFAULT CURRENT_DATE,
    status VARCHAR(20) NOT NULL, -- 'PRESENT', 'ABSENT', 'HALF_DAY', 'LEAVE'
    check_in TIME,
    check_out TIME,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT unique_staff_attendance UNIQUE (staff_id, work_date)
);

CREATE TABLE staff_salaries (
    id SERIAL PRIMARY KEY,
    staff_id INT REFERENCES staff(id) ON DELETE CASCADE,
    payment_date DATE NOT NULL,
    amount_paid DECIMAL(10, 2) NOT NULL,
    payment_mode VARCHAR(50) NOT NULL, -- 'CASH', 'BANK_TRANSFER'
    remarks TEXT
);

-- ============================================================================
-- 8. EMAIL TEMPLATES
-- ============================================================================

CREATE TABLE email_templates (
    id SERIAL PRIMARY KEY,
    template_key VARCHAR(50) NOT NULL,
    notification_type INT NOT NULL DEFAULT 1,
    display_name VARCHAR(100) NOT NULL,
    description VARCHAR(500) NOT NULL,
    subject VARCHAR(255) NOT NULL,
    headline VARCHAR(255) NOT NULL,
    body_html TEXT NOT NULL,
    preheader VARCHAR(255),
    button_label VARCHAR(100),
    button_link_token VARCHAR(100),
    available_tokens TEXT NOT NULL DEFAULT '[]',
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE (template_key, notification_type)
);
