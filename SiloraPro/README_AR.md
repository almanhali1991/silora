# SiloraPro - نظام إدارة متكامل

## نظرة عامة
سيلورا برو هو نظام إدارة متكامل مبني باستخدام أحدث التقنيات:

- **اللغة**: C#
- **الإطار**: .NET 10 LTS
- **واجهة المستخدم**: WPF
- **العمارة**: MVVM + Clean Architecture
- **قاعدة البيانات**: SQLite مع Entity Framework Core
- **توليد PDF**: QuestPDF
- **التسجيل**: Serilog

## هيكل المشروع
```
SiloraPro/
├── src/
│   ├── SiloraPro.Domain/          # الكيانات والنماذج
│   ├── SiloraPro.Application/     # الخدمات وواجهات البرمجة
│   ├── SiloraPro.Infrastructure/  # الوصول للبيانات والمستودعات
│   └── SiloraPro.Presentation/    # واجهة المستخدم WPF
└── assets/                        # الموارد والأيقونات
```

## المميزات
- ✅ إدارة العملاء
- ✅ إدارة المنتجات والمخزون
- ✅ إدارة الطلبات
- ✅ تقارير PDF
- ✅ تصميم عربي حديث
- ✅ دعم كامل للغة العربية

## متطلبات التشغيل
- .NET 10 SDK
- Windows 10/11 (لتشغيل WPF)

## التثبيت والتشغيل
1. افتح الملف `SiloraPro.sln` في Visual Studio 2022 أو JetBrains Rider
2. استعد الحزم NuGet
3. شغل المشروع `SiloraPro.Presentation`

## التطوير
المشروع يتبع مبادئ Clean Architecture و MVVM Pattern لتسهيل الصيانة والتوسع.

---
**حقوق النشر © 2024 سيلورا برو**
