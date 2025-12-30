# Snaa Platform API (صناع)

هذا هو الـ Back-end API لمشروع **صناع (Snaa Platform)**، مبني باستخدام **.NET 8** و **C#**، مع التكامل مع **Supabase** لإدارة قاعدة البيانات والتخزين.

## المتطلبات التقنية
- .NET 8 SDK
- حساب Supabase
- Docker (اختياري للنشر)

## إعداد قاعدة البيانات (Supabase)
يجب إنشاء الجداول التالية في Supabase SQL Editor:

```sql
-- جدول المستخدمين
create table users (
  id uuid primary key default uuid_generate_v4(),
  email text unique not null,
  full_name text not null,
  account_type text not null, -- 'Individual' or 'Facility'
  commercial_registration text,
  specialization text,
  password_hash text not null,
  created_at timestamp with time zone default now()
);

-- إعداد التخزين (Storage)
-- قم بإنشاء Bucket جديد باسم 'designs' واجعله Public.
```

## الإعدادات (appsettings.json)
قم بتحديث القيم التالية في ملف `appsettings.json`:
- `Supabase:Url`: رابط مشروعك في Supabase.
- `Supabase:Key`: مفتاح الـ Anon Key الخاص بك.
- `JwtSettings:Key`: مفتاح سري قوي لتشفير الـ JWT.

## التشغيل باستخدام Docker
لبناء وتشغيل المشروع باستخدام Docker:
```bash
docker build -t snaa-api .
docker run -p 8080:80 snaa-api
```

## النشر على Render
1. ارفع الكود إلى GitHub.
2. اختر "Web Service" في Render.
3. اربط المستودع واختر "Docker" كـ Runtime.
4. أضف المتغيرات البيئية (Environment Variables) المطابقة لـ `appsettings.json`.
