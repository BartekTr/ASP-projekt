USE [ASP_Projekt]
GO
SET IDENTITY_INSERT [dbo].[Company] ON 

INSERT [dbo].[Company] ([Id], [Name], [Description]) VALUES (1, N'Daftcode', N'Dobra firma')
INSERT [dbo].[Company] ([Id], [Name], [Description]) VALUES (2, N'Asseco', N'Œrednia firma')
INSERT [dbo].[Company] ([Id], [Name], [Description]) VALUES (3, N'Najlepsza_Firma', N'Dobrze p³aci')
SET IDENTITY_INSERT [dbo].[Company] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Id], [Name]) VALUES (1, N'Admin')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (2, N'HR')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (3, N'User')
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [FirstName], [LastName], [EmailAdress], [RoleId], [PhoneNumber]) VALUES (2, N'Bartek', N'Truszkowski', N'b@t.pl', 1, N'123312331')
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[JobOffer] ON 

INSERT [dbo].[JobOffer] ([Id], [JobTitle], [CompanyId], [SalaryFrom], [SalaryTo], [Created], [Location], [Description], [ValidUntil], [HRId]) VALUES (3, N'Programmer', 1, CAST(10.00 AS Decimal(18, 2)), CAST(11.00 AS Decimal(18, 2)), CAST(N'2019-11-13T23:48:40.7512607' AS DateTime2), N'dsadas', N'dasdsx', CAST(N'2019-11-13T23:48:40.7512607' AS DateTime2), 2)
INSERT [dbo].[JobOffer] ([Id], [JobTitle], [CompanyId], [SalaryFrom], [SalaryTo], [Created], [Location], [Description], [ValidUntil], [HRId]) VALUES (4, N'new offer22', 3, CAST(100.00 AS Decimal(18, 2)), CAST(321.00 AS Decimal(18, 2)), CAST(N'2020-01-02T22:58:30.9215013' AS DateTime2), N'Gdynia', N'dasdsa', NULL, NULL)
INSERT [dbo].[JobOffer] ([Id], [JobTitle], [CompanyId], [SalaryFrom], [SalaryTo], [Created], [Location], [Description], [ValidUntil], [HRId]) VALUES (7, N'dsadsa', 1, CAST(21.00 AS Decimal(18, 2)), CAST(212.00 AS Decimal(18, 2)), CAST(N'2020-01-03T20:41:10.8863983' AS DateTime2), N'adsad', N'dasdas', NULL, NULL)
INSERT [dbo].[JobOffer] ([Id], [JobTitle], [CompanyId], [SalaryFrom], [SalaryTo], [Created], [Location], [Description], [ValidUntil], [HRId]) VALUES (8, N'new offer2', 2, CAST(123.00 AS Decimal(18, 2)), CAST(200.00 AS Decimal(18, 2)), CAST(N'2020-01-04T01:56:43.5077439' AS DateTime2), N'Gdynia', N'dsa', NULL, NULL)
SET IDENTITY_INSERT [dbo].[JobOffer] OFF
SET IDENTITY_INSERT [dbo].[State] ON 

INSERT [dbo].[State] ([Id], [Name]) VALUES (1, N'Pending')
INSERT [dbo].[State] ([Id], [Name]) VALUES (2, N'Accepted')
INSERT [dbo].[State] ([Id], [Name]) VALUES (3, N'Rejected')
SET IDENTITY_INSERT [dbo].[State] OFF

SET IDENTITY_INSERT [dbo].[JobApplication] ON 

INSERT [dbo].[JobApplication] ([Id], [OfferId], [ContactAgreement], [CvUrl], [UserId], [StateId]) VALUES (1, 4, 1, N'dsa', 2, 1)
INSERT [dbo].[JobApplication] ([Id], [OfferId], [ContactAgreement], [CvUrl], [UserId], [StateId]) VALUES (3, 3, 1, N'P³k. D¹bka 167E/8', 2, 1)
SET IDENTITY_INSERT [dbo].[JobApplication] OFF


