---
version: 1.0
name: QLSYLL — Hệ thống Quản lý Sơ yếu lý lịch
description: A clean, white-dominant HR management interface built on pure white canvas with Professional Blue (#2563eb) as the single brand accent. The design language prioritizes data clarity, form readability, and administrative efficiency. White surfaces dominate 90%+ of every page — sidebar, cards, tables, and forms all sit on white or near-white backgrounds. Blue appears only on primary CTAs, active sidebar items, and key status indicators. Type runs Inter at readable weights — page titles at 20–24px / 600, table headers at 14px / 600, body at 14–16px / 400. The system feels like a modern SaaS admin panel — clean, functional, trustworthy.

colors:
  primary: "#2563eb"
  primary-hover: "#1d4ed8"
  primary-active: "#1e40af"
  primary-light: "#eff6ff"
  primary-muted: "#93c5fd"
  primary-50: "#eff6ff"
  primary-100: "#dbeafe"
  sidebar-bg: "#ffffff"
  sidebar-active-bg: "#eff6ff"
  sidebar-text: "#64748b"
  sidebar-active-text: "#2563eb"
  sidebar-icon: "#94a3b8"
  sidebar-active-icon: "#2563eb"
  success: "#16a34a"
  success-light: "#f0fdf4"
  warning: "#d97706"
  warning-light: "#fffbeb"
  error: "#dc2626"
  error-light: "#fef2f2"
  info: "#0284c7"
  info-light: "#f0f9ff"
  ink: "#1e293b"
  body: "#475569"
  muted: "#64748b"
  muted-soft: "#94a3b8"
  placeholder: "#cbd5e1"
  hairline: "#e2e8f0"
  hairline-soft: "#f1f5f9"
  border-focus: "#2563eb"
  canvas: "#f8fafc"
  surface-card: "#ffffff"
  surface-white: "#ffffff"
  surface-header: "#ffffff"
  table-header-bg: "#f8fafc"
  table-row-hover: "#f8fafc"
  table-stripe: "#fafbfc"
  on-primary: "#ffffff"
  avatar-bg: "#dbeafe"
  avatar-text: "#2563eb"
  scrim: "#000000"

typography:
  page-title:
    fontFamily: "'Inter', -apple-system, system-ui, 'Segoe UI', sans-serif"
    fontSize: 24px
    fontWeight: 600
    lineHeight: 1.33
    letterSpacing: -0.24px
  section-title:
    fontFamily: "'Inter', sans-serif"
    fontSize: 18px
    fontWeight: 600
    lineHeight: 1.44
    letterSpacing: 0
  card-title:
    fontFamily: "'Inter', sans-serif"
    fontSize: 16px
    fontWeight: 600
    lineHeight: 1.5
    letterSpacing: 0
  body-md:
    fontFamily: "'Inter', sans-serif"
    fontSize: 14px
    fontWeight: 400
    lineHeight: 1.57
    letterSpacing: 0
  body-sm:
    fontFamily: "'Inter', sans-serif"
    fontSize: 13px
    fontWeight: 400
    lineHeight: 1.46
    letterSpacing: 0
  label:
    fontFamily: "'Inter', sans-serif"
    fontSize: 14px
    fontWeight: 500
    lineHeight: 1.43
    letterSpacing: 0
  table-header:
    fontFamily: "'Inter', sans-serif"
    fontSize: 12px
    fontWeight: 600
    lineHeight: 1.33
    letterSpacing: 0.4px
    textTransform: uppercase
  table-cell:
    fontFamily: "'Inter', sans-serif"
    fontSize: 14px
    fontWeight: 400
    lineHeight: 1.43
    letterSpacing: 0
  stat-number:
    fontFamily: "'Inter', sans-serif"
    fontSize: 28px
    fontWeight: 700
    lineHeight: 1.14
    letterSpacing: -0.28px
  stat-label:
    fontFamily: "'Inter', sans-serif"
    fontSize: 13px
    fontWeight: 400
    lineHeight: 1.38
    letterSpacing: 0
  button-md:
    fontFamily: "'Inter', sans-serif"
    fontSize: 14px
    fontWeight: 500
    lineHeight: 1.29
    letterSpacing: 0
  button-sm:
    fontFamily: "'Inter', sans-serif"
    fontSize: 13px
    fontWeight: 500
    lineHeight: 1.23
    letterSpacing: 0
  nav-link:
    fontFamily: "'Inter', sans-serif"
    fontSize: 14px
    fontWeight: 500
    lineHeight: 1.43
    letterSpacing: 0
  breadcrumb:
    fontFamily: "'Inter', sans-serif"
    fontSize: 13px
    fontWeight: 400
    lineHeight: 1.38
    letterSpacing: 0
  badge:
    fontFamily: "'Inter', sans-serif"
    fontSize: 11px
    fontWeight: 600
    lineHeight: 1.18
    letterSpacing: 0.2px

rounded:
  none: 0px
  xs: 4px
  sm: 6px
  md: 8px
  lg: 12px
  xl: 16px
  full: 9999px

spacing:
  xxs: 2px
  xs: 4px
  sm: 8px
  md: 12px
  base: 16px
  lg: 24px
  xl: 32px
  xxl: 48px

components:
  button-primary:
    backgroundColor: "{colors.primary}"
    textColor: "{colors.on-primary}"
    typography: "{typography.button-md}"
    rounded: "{rounded.md}"
    padding: 8px 16px
    height: 36px
  button-primary-hover:
    backgroundColor: "{colors.primary-hover}"
  button-secondary:
    backgroundColor: "{colors.surface-white}"
    textColor: "{colors.ink}"
    typography: "{typography.button-md}"
    rounded: "{rounded.md}"
    padding: 7px 15px
    height: 36px
    border: "1px solid {colors.hairline}"
  button-secondary-hover:
    backgroundColor: "{colors.table-header-bg}"
    border: "1px solid {colors.muted-soft}"
  button-danger:
    backgroundColor: "{colors.error}"
    textColor: "{colors.on-primary}"
    typography: "{typography.button-md}"
    rounded: "{rounded.md}"
    height: 36px
  button-ghost:
    backgroundColor: transparent
    textColor: "{colors.primary}"
    typography: "{typography.button-md}"
  button-icon:
    backgroundColor: transparent
    textColor: "{colors.muted}"
    rounded: "{rounded.md}"
    height: 32px
    width: 32px
  sidebar:
    backgroundColor: "{colors.sidebar-bg}"
    width: 260px
    borderRight: "1px solid {colors.hairline}"
    padding: "16px 0"
  sidebar-logo:
    height: 56px
    padding: "0 20px"
  sidebar-item:
    backgroundColor: transparent
    textColor: "{colors.sidebar-text}"
    typography: "{typography.nav-link}"
    padding: "10px 20px"
    rounded: "{rounded.md}"
    margin: "2px 12px"
    iconSize: 20px
    iconColor: "{colors.sidebar-icon}"
  sidebar-item-active:
    backgroundColor: "{colors.sidebar-active-bg}"
    textColor: "{colors.sidebar-active-text}"
    iconColor: "{colors.sidebar-active-icon}"
    fontWeight: 600
  sidebar-item-hover:
    backgroundColor: "{colors.hairline-soft}"
  top-header:
    backgroundColor: "{colors.surface-header}"
    height: 56px
    borderBottom: "1px solid {colors.hairline}"
    padding: "0 24px"
  data-table:
    backgroundColor: "{colors.surface-white}"
    rounded: "{rounded.lg}"
    border: "1px solid {colors.hairline}"
    overflow: hidden
  table-head:
    backgroundColor: "{colors.table-header-bg}"
    textColor: "{colors.muted}"
    typography: "{typography.table-header}"
    padding: "12px 16px"
  table-row:
    backgroundColor: "{colors.surface-white}"
    textColor: "{colors.ink}"
    typography: "{typography.table-cell}"
    padding: "12px 16px"
    borderBottom: "1px solid {colors.hairline-soft}"
  table-row-hover:
    backgroundColor: "{colors.table-row-hover}"
  stat-card:
    backgroundColor: "{colors.surface-white}"
    rounded: "{rounded.lg}"
    padding: "24px"
    border: "1px solid {colors.hairline}"
  form-card:
    backgroundColor: "{colors.surface-white}"
    rounded: "{rounded.lg}"
    padding: "24px"
    border: "1px solid {colors.hairline}"
  text-input:
    backgroundColor: "{colors.surface-white}"
    textColor: "{colors.ink}"
    typography: "{typography.body-md}"
    rounded: "{rounded.md}"
    padding: "8px 12px"
    height: 38px
    border: "1px solid {colors.hairline}"
  text-input-focus:
    border: "2px solid {colors.border-focus}"
    boxShadow: "0 0 0 3px rgba(37, 99, 235, 0.1)"
  select-input:
    backgroundColor: "{colors.surface-white}"
    textColor: "{colors.ink}"
    rounded: "{rounded.md}"
    height: 38px
    border: "1px solid {colors.hairline}"
  badge-success:
    backgroundColor: "{colors.success-light}"
    textColor: "{colors.success}"
    typography: "{typography.badge}"
    rounded: "{rounded.full}"
    padding: "2px 8px"
  badge-warning:
    backgroundColor: "{colors.warning-light}"
    textColor: "{colors.warning}"
    typography: "{typography.badge}"
    rounded: "{rounded.full}"
    padding: "2px 8px"
  badge-error:
    backgroundColor: "{colors.error-light}"
    textColor: "{colors.error}"
    typography: "{typography.badge}"
    rounded: "{rounded.full}"
    padding: "2px 8px"
  badge-info:
    backgroundColor: "{colors.info-light}"
    textColor: "{colors.info}"
    typography: "{typography.badge}"
    rounded: "{rounded.full}"
    padding: "2px 8px"
  avatar:
    backgroundColor: "{colors.avatar-bg}"
    textColor: "{colors.avatar-text}"
    rounded: "{rounded.full}"
    width: 36px
    height: 36px
    typography: "{typography.button-sm}"
  avatar-lg:
    width: 80px
    height: 80px
    fontSize: 28px
  pagination:
    backgroundColor: "{colors.surface-white}"
    textColor: "{colors.muted}"
    rounded: "{rounded.md}"
    height: 32px
    padding: "0 12px"
    border: "1px solid {colors.hairline}"
  pagination-active:
    backgroundColor: "{colors.primary}"
    textColor: "{colors.on-primary}"
  search-input:
    backgroundColor: "{colors.surface-white}"
    textColor: "{colors.ink}"
    rounded: "{rounded.md}"
    height: 36px
    padding: "0 12px 0 36px"
    border: "1px solid {colors.hairline}"
    iconColor: "{colors.muted-soft}"
  notification-toast:
    backgroundColor: "{colors.surface-white}"
    rounded: "{rounded.lg}"
    padding: "12px 16px"
    boxShadow: "0 4px 12px rgba(0,0,0,0.1)"
  modal:
    backgroundColor: "{colors.surface-white}"
    rounded: "{rounded.lg}"
    padding: "24px"
    boxShadow: "0 8px 32px rgba(0,0,0,0.12)"
  login-card:
    backgroundColor: "{colors.surface-white}"
    rounded: "{rounded.xl}"
    padding: "40px"
    boxShadow: "0 1px 3px rgba(0,0,0,0.06), 0 4px 16px rgba(0,0,0,0.06)"
    maxWidth: 420px
---

## Overview

**QLSYLL** is an internal HR management system designed for data clarity and administrative efficiency. The visual system is built on a **white-dominant canvas** — over 90% of every page is white (`{colors.surface-white}` — #ffffff) or near-white (`{colors.canvas}` — #f8fafc). The single brand accent is **Professional Blue** (`{colors.primary}` — #2563eb), used sparingly on primary CTAs, the active sidebar indicator, stat card accents, and the login page logo.

The **logo** is the text **"QLSYLL"** or a stylized document/person icon rendered in `{colors.primary}` — clean and functional, not decorative.

Type runs **Inter** at readable, functional weights — page titles at 24px / 600 are the largest element in the admin layout (no hero headlines needed in a management tool); table headers at 12px / 600 uppercase carry structural authority; body and table cells sit at 14px / 400 for data density. The type scale is deliberately restrained — this is a tool for reading data, not selling a product.

The shape language is **softly rectangular**. Buttons and inputs use 8px radius (`{rounded.md}`), cards and tables use 12px radius (`{rounded.lg}`). No fully-rounded elements except avatars and badges. The system feels professional and orderly.

**Key Characteristics:**
- **White everywhere:** Sidebar, header, cards, tables, forms — all white. The only non-white surface is the page canvas itself at `{colors.canvas}` (#f8fafc), a barely-visible warm gray that makes white cards pop.
- **One accent color:** `{colors.primary}` (#2563eb) appears on: primary buttons, active sidebar item background tint + text, focus rings on inputs, stat numbers on dashboard cards, and the login logo. Nowhere else.
- **Data-dense tables:** `{component.data-table}` uses compact 12px padding, uppercase 12px headers in muted gray, and hover-highlight rows for scanability.
- **Sidebar navigation:** White background, 260px wide, separated by a 1px hairline. Items are text + 20px icon. Active item gets a light blue background tint (`{colors.primary-light}` — #eff6ff) with blue text + icon.
- **Dashboard stat cards:** White cards with 1px border, containing a `{typography.stat-number}` (28px / 700) in blue and a `{typography.stat-label}` in muted gray.
- **Form-heavy pages:** Resume create/edit forms use white `{component.form-card}` with clear label → input stacking, 38px input height, and blue focus rings.

## Colors

### Brand
- **Professional Blue** (`{colors.primary}` — #2563eb): The single brand accent. Primary buttons, active sidebar, focus rings, stat numbers, login logo.
- **Blue Hover** (`{colors.primary-hover}` — #1d4ed8): Button hover state.
- **Blue Active** (`{colors.primary-active}` — #1e40af): Button press state.
- **Blue Light** (`{colors.primary-light}` — #eff6ff): Active sidebar item background, badge backgrounds, subtle section tints.
- **Blue 100** (`{colors.primary-100}` — #dbeafe): Avatar default background.
- **Blue Muted** (`{colors.primary-muted}` — #93c5fd): Disabled button fills, progress bar tracks.

### Surfaces (All White-Based)
- **Canvas** (`{colors.canvas}` — #f8fafc): The page floor — barely gray, making white cards distinct.
- **Surface White** (`{colors.surface-white}` — #ffffff): Sidebar, header, cards, tables, forms, modals — everything interactive sits on pure white.
- **Table Header** (`{colors.table-header-bg}` — #f8fafc): Matches canvas — header row is visually distinct from white body rows.
- **Table Row Hover** (`{colors.table-row-hover}` — #f8fafc): Subtle highlight on pointer hover.

### Sidebar
- **Sidebar BG** (`{colors.sidebar-bg}` — #ffffff): Pure white sidebar.
- **Sidebar Text** (`{colors.sidebar-text}` — #64748b): Inactive menu item text — medium gray.
- **Sidebar Active Text** (`{colors.sidebar-active-text}` — #2563eb): Active item — blue.
- **Sidebar Active BG** (`{colors.sidebar-active-bg}` — #eff6ff): Active item background — light blue tint.

### Borders
- **Hairline** (`{colors.hairline}` — #e2e8f0): Default 1px border on cards, inputs, table outlines.
- **Hairline Soft** (`{colors.hairline-soft}` — #f1f5f9): Table row separators, light dividers.

### Text
- **Ink** (`{colors.ink}` — #1e293b): Page titles, table cell primary data, form labels.
- **Body** (`{colors.body}` — #475569): Descriptions, secondary information.
- **Muted** (`{colors.muted}` — #64748b): Table headers, sidebar inactive text, timestamps.
- **Placeholder** (`{colors.placeholder}` — #cbd5e1): Input placeholder text.

### Semantic Status
- **Success** (`{colors.success}` — #16a34a on `{colors.success-light}` — #f0fdf4): "Hoạt động", "Đã duyệt".
- **Warning** (`{colors.warning}` — #d97706 on `{colors.warning-light}` — #fffbeb): "Chờ xử lý", "Sắp hết hạn".
- **Error** (`{colors.error}` — #dc2626 on `{colors.error-light}` — #fef2f2): "Bị khóa", "Lỗi", form validation.
- **Info** (`{colors.info}` — #0284c7 on `{colors.info-light}` — #f0f9ff): "Thông tin", "Mới".

## Layout

### Admin Layout Structure
```
┌─────────────────────────────────────────────────────────────────┐
│ [Sidebar 260px]  │  [Header 56px]                              │
│                  │─────────────────────────────────────────────│
│  Logo            │  Breadcrumb          🔔  👤 Admin ▼        │
│                  │                                             │
│  📊 Dashboard    │  ┌─────────────────────────────────────┐   │
│  📋 Hồ sơ SYLL  │  │                                     │   │
│  👤 Tài khoản   │  │         CONTENT AREA                │   │
│  🏢 Phòng ban   │  │       (white cards on                │   │
│  💼 Chức vụ     │  │        #f8fafc canvas)               │   │
│  📢 Thông báo   │  │                                     │   │
│  ──────────     │  │                                     │   │
│  🔑 Đổi MK      │  └─────────────────────────────────────┘   │
│                  │                                             │
│  [White BG]      │  [#f8fafc BG]                              │
└─────────────────────────────────────────────────────────────────┘
```

### Spacing
- **Base unit:** 4px.
- **Page padding:** 24px around the content area.
- **Card padding:** 24px internal.
- **Card gap:** 24px between dashboard stat cards.
- **Table cell padding:** 12px horizontal, 12px vertical.
- **Form field gap:** 16px between label-input pairs.
- **Section gap:** 24px between content sections.

### Grid
- **Sidebar:** Fixed 260px. Collapses to icon-only 64px on tablet, hidden on mobile.
- **Content max-width:** Fluid, fills remaining space. Content cards max at ~1200px.
- **Dashboard stat cards:** 4-column at desktop, 2×2 on tablet, 1-column stacked on mobile.
- **Form layout:** 2-column (label left 30%, input right 70%) on desktop; 1-column stacked on mobile.
- **Resume detail:** Single column, sections stacked vertically with white card per section.

## Elevation

Minimal shadows — the white-on-gray-canvas separation provides enough depth:

- **Flat:** Sidebar, header — separated by 1px hairline borders, no shadow.
- **Card rest:** No shadow by default — cards are distinguished by white surface on gray canvas + 1px border.
- **Login card:** `box-shadow: 0 1px 3px rgba(0,0,0,0.06), 0 4px 16px rgba(0,0,0,0.06)` — the only page where a card floats prominently.
- **Dropdown/Modal:** `box-shadow: 0 8px 32px rgba(0,0,0,0.12)` — account menu, confirm dialogs.
- **Toast:** `box-shadow: 0 4px 12px rgba(0,0,0,0.1)` — success/error notification popups.

## Components

### Sidebar
**`sidebar`** — White background, 260px width, 1px right border. Logo area at top (56px height). Menu items vertically stacked with 20px icons and 14px / 500 labels. Active item: light blue bg tint + blue text + blue icon. Hover: `{colors.hairline-soft}` background.

### Header
**`top-header`** — White surface, 56px height, 1px bottom border. Left: breadcrumb trail. Right: notification bell icon + user avatar + name dropdown.

### Data Tables
**`data-table`** — White surface, 12px radius, 1px border. Header row on `{colors.table-header-bg}` with 12px uppercase muted labels. Body rows on white with 1px soft hairline separators. Last column holds action icons (view 👁, edit ✏️, delete 🗑️). Hover row highlights to `{colors.table-row-hover}`.

### Dashboard Stat Cards
**`stat-card`** — White surface, 12px radius, 1px border, 24px padding. Contains: icon (40px, blue-tinted), stat number in `{typography.stat-number}` blue, and label in `{typography.stat-label}` muted. Four cards across: "Tổng nhân viên", "Phòng ban", "Thông báo mới", "Hồ sơ chờ".

### Forms
**`text-input`** — White surface, 1px hairline border, 8px radius, 38px height. On focus: 2px blue border with soft blue outer glow. Labels above in `{typography.label}` weight 500.

**`select-input`** — Same styling as text-input with dropdown arrow icon.

### Badges (Status)
Status badges use semantic color pairs (text on light background), fully rounded pills:
- **Active/Success:** Green text on green-light bg — "Hoạt động", "Đã duyệt"
- **Warning:** Amber text on amber-light bg — "Chờ xử lý"
- **Locked/Error:** Red text on red-light bg — "Bị khóa"
- **Info:** Blue text on blue-light bg — "Mới"

### Buttons
**`button-primary`** — Blue fill (#2563eb), white text, 8px radius, 36px height. Main actions: "Thêm mới", "Lưu", "Tìm kiếm".

**`button-secondary`** — White fill, ink text, 1px hairline border. Secondary actions: "Hủy", "Quay lại", "Xuất".

**`button-danger`** — Red fill, white text. Destructive: "Xóa", "Khóa tài khoản".

**`button-ghost`** — No fill, blue text. Inline: "Xem thêm", "Chỉnh sửa".

### Avatar
**`avatar`** — 36px circle, blue-light bg (`{colors.avatar-bg}` — #dbeafe), blue text initials. Used in header user area and employee directory. Large variant: 80px for resume detail page.

### Pagination
**`pagination`** — Row of 32px square buttons with 1px border. Active page: blue fill, white text. Inactive: white fill, muted text. Placed below data tables.

### Login Page
**`login-card`** — White surface, 16px radius, 40px padding, subtle shadow. Centered on a `{colors.canvas}` full-page background. Contains: logo + system name, username input, password input, "Nhớ tài khoản" checkbox, "Đăng nhập" primary button full-width.

### Modal (Confirm Delete)
**`modal`** — White surface, 12px radius, 24px padding, deep shadow. Scrim at 50% black. Contains: warning icon, confirmation text, "Hủy" secondary + "Xóa" danger buttons.

## Responsive Behavior

| Breakpoint | Width | Key Changes |
|---|---|---|
| Mobile | < 768px | Sidebar hidden, hamburger icon in header; tables scroll horizontally; form inputs stack 1-column; stat cards stack vertically; login card fills screen width with 16px margin. |
| Tablet | 768–1024px | Sidebar collapses to icon-only (64px wide); content area expands; stat cards 2×2; tables visible with horizontal scroll if needed. |
| Desktop | ≥ 1024px | Full sidebar (260px) + content area; stat cards 4-up; forms 2-column; tables fully visible. |

## Known Gaps

- **Dark mode:** Not planned — HR admin systems are daylight-use tools.
- **Print styles:** Resume detail page should have a print-optimized CSS — not documented here.
- **Chart/Graph styling:** Dashboard may add charts later — chart color palette not specified.
- **CKEditor theme:** The rich text editor (Announcements) needs custom CSS to match the system font and spacing — not documented here.
