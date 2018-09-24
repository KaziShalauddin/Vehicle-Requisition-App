

INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'18f35301-c1a2-426a-88bb-52647ec7752e', N'Controller')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'648d557d-307b-4a72-9555-5f60070d80c9', N'Employee')

INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'5d32f8bb-ba19-4846-8115-c0fb3b858053', N'a@b.com', 0, N'ACPBxQ8IJI1z63/yIBZLRwh54U4QU0QDDTT7mwgJlHKuc5nZAzSXoFCnx46X/EoV6g==', N'a6c8dfc3-cf07-4b02-b6d2-390a27462332', NULL, 0, 0, NULL, 1, 0, N'a@b.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'9515064e-b701-4672-a23a-dda031fbb745', N'b@c.com', 0, N'ABD+1v6rW2hQjJkjvdK/DRBfvZpqgHQUcmelcQIcVNWKugmlKHSbecRGheGtdJkERg==', N'8292030c-fbd3-4b9e-91d3-95691509ffc7', NULL, 0, 0, NULL, 1, 0, N'b@c.com')
INSERT [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'bd541c3b-66fa-4471-bc78-c54d2898565c', N'x@y.com', 0, N'ALAeFEmHpkm6tgyDCLFOJsFjA1FbXBqvtN72DRXdjvHhkxnzaCo8xDaFp6Rfzj5WFw==', N'cb0555c1-3d01-4de3-9b78-fad142119e33', NULL, 0, 0, NULL, 1, 0, N'x@y.com')


INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5d32f8bb-ba19-4846-8115-c0fb3b858053', N'18f35301-c1a2-426a-88bb-52647ec7752e')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'9515064e-b701-4672-a23a-dda031fbb745', N'18f35301-c1a2-426a-88bb-52647ec7752e')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'bd541c3b-66fa-4471-bc78-c54d2898565c', N'648d557d-307b-4a72-9555-5f60070d80c9')


INSERT [dbo].[Organaizations] ( [Name], [Description], [IsDeleted]) VALUES ( N'Ministry of ICT', NULL, 0)

INSERT [dbo].[Departments] ( [Name], [OrganaizationId], [IsDeleted]) VALUES ( N'BKIICT', 1, 0)


INSERT [dbo].[Designations] ( [Name], [OrganaizationId], [IsDeleted]) VALUES ( N'Officer', 1, 0)
INSERT [dbo].[Designations] ( [Name], [OrganaizationId], [IsDeleted]) VALUES ( N'System Admin', 1, 0)
 


INSERT [dbo].[Divisions] ( [Name], [IsDeleted]) VALUES ( N'Dhaka', 0)
INSERT [dbo].[Divisions] ( [Name], [IsDeleted]) VALUES ( N'Khulna', 0)


INSERT [dbo].[Districts] ( [Name], [DivisionId], [IsDeleted]) VALUES ( N'Dhaka', 1, 0)
INSERT [dbo].[Districts] ( [Name], [DivisionId], [IsDeleted]) VALUES ( N'Jessore', 2, 0)

INSERT [dbo].[Thanas] ( [Name], [DistrictId], [IsDeleted]) VALUES ( N'Mirpur', 1, 0)
INSERT [dbo].[Thanas] ( [Name], [DistrictId], [IsDeleted]) VALUES ( N'Noapara', 2, 0)
INSERT [dbo].[Thanas] ( [Name], [DistrictId], [IsDeleted]) VALUES ( N'Shah Ali', 1, 0) 




INSERT [dbo].[Employees] ( [UserId], [Name], [ContactNo], [Email], [Address1], [Address2], [LicenceNo], [IsDriver], [DepartmentId], [DesignationId], [DivisionId], [DistrictId], [ThanaId], [IsDeleted]) VALUES ( NULL, NULL, NULL, N'x@y.com', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, 0)


INSERT [dbo].[Requsitions] ( [Form], [To], [Description], [JourneyStart], [JouneyEnd], [Status], [EmployeeId], [IsDeleted]) VALUES ( N'BCC', N'Savar', N'New', CAST(N'2018-09-20T10:00:00.000' AS DateTime), CAST(N'2018-09-20T05:00:00.000' AS DateTime), NULL, 1, 0)
INSERT [dbo].[Requsitions] ( [Form], [To], [Description], [JourneyStart], [JouneyEnd], [Status], [EmployeeId], [IsDeleted]) VALUES ( N'BCC', N'Mirpur-10', N'New', CAST(N'2018-09-20T13:00:00.000' AS DateTime), CAST(N'2018-09-20T08:00:00.000' AS DateTime), NULL, 1, 0)
INSERT [dbo].[Requsitions] ( [Form], [To], [Description], [JourneyStart], [JouneyEnd], [Status], [EmployeeId], [IsDeleted]) VALUES ( N'BCC', N'Lalbag', N'New', CAST(N'2018-09-19T09:00:00.000' AS DateTime), CAST(N'2018-09-19T20:00:00.000' AS DateTime), NULL, 1, 0)
INSERT [dbo].[Requsitions] ( [Form], [To], [Description], [JourneyStart], [JouneyEnd], [Status], [EmployeeId], [IsDeleted]) VALUES ( N'BCC', N'Lalbag', N'New', CAST(N'2018-09-19T13:00:00.000' AS DateTime), CAST(N'2018-09-19T22:00:00.000' AS DateTime), NULL, 1, 0)
INSERT [dbo].[Requsitions] ( [Form], [To], [Description], [JourneyStart], [JouneyEnd], [Status], [EmployeeId], [IsDeleted]) VALUES ( N'BCC', N'Mirpur', N'New', CAST(N'2018-09-19T13:00:00.000' AS DateTime), CAST(N'2018-09-19T14:00:00.000' AS DateTime), N'Complete', 1, 0)


