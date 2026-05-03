SELECT 'Service Level' as 'Key', [Service Level] as 'Value'  FROM [dbo].[Service Level] WHERE [Service Level] IS NOT NULL AND [Service Level] <> ''
union
SELECT 'Service Type', [Type] FROM [dbo].[Service Type] WHERE [Type] IS NOT NULL AND [Type] <> ''
union
SELECT 'Service', [Service] FROM [dbo].[Service] WHERE [Service] IS NOT NULL AND [Service] <> ''
union
SELECT 'Service Category', [Service Type] FROM [dbo].[Service Category] WHERE [Service Type] IS NOT NULL AND [Service Type] <> ''
union
SELECT 'Service Subcategory', [Service Subcategory] FROM [dbo].[Service Subcategory] WHERE [Service Subcategory] IS NOT NULL AND [Service Subcategory] <> ''
union
SELECT 'Age Group', [Age Group] FROM [dbo].[Age Groups] WHERE [Age Group] IS NOT NULL AND [Age Group] <> ''
union
SELECT 'Insurance Company', [Insurance Company] FROM [dbo].[Insurance Company] WHERE [Insurance Company] IS NOT NULL AND [Insurance Company] <> ''
union
SELECT 'Insurance Plan', [Plan] FROM [dbo].[Insurance Plans] WHERE [Plan] IS NOT NULL AND [Plan] <> ''
union
SELECT 'Company', [Company] FROM [dbo].[Company] WHERE [Company] IS NOT NULL AND [Company] <> ''
union
SELECT 'Location', [Locations] FROM [dbo].[Locations] WHERE [Locations] IS NOT NULL AND [Locations] <> ''
union
SELECT 'County', [County] FROM [dbo].[County] WHERE [County] IS NOT NULL AND [County] <> ''
union
SELECT 'Street', [Street] FROM [dbo].[Locations] WHERE [Street] IS NOT NULL AND [Street] <> ''
union
SELECT 'Suite/Building', [St/bldg] FROM [dbo].[Locations] WHERE [St/bldg] IS NOT NULL AND [St/bldg] <> ''
union
SELECT 'City', [City] FROM [dbo].[City] WHERE [City] IS NOT NULL AND [City] <> ''
union
SELECT 'State', [State] FROM [dbo].[State] WHERE [State] IS NOT NULL AND [State] <> ''
union
SELECT 'Zip', TRIM([Zip]) FROM [dbo].[Zip] WHERE [Zip] IS NOT NULL AND [Zip] <> ''
union
SELECT 'Phone', [Phone] FROM [dbo].[Locations] WHERE [Phone] IS NOT NULL AND [Phone] <> ''
union
SELECT 'Language', [Language] FROM [dbo].[Language] WHERE [Language] IS NOT NULL AND [Language] <> ''
order by [Key]