/*
  Витринные цены для учебной базы SneakerShop.
  Выполни в SSMS, выбрав базу SneakerShop (или раскомментируй USE).
*/
-- USE SneakerShop;
-- GO

UPDATE dbo.Products SET Price = 12990.00 WHERE ProductName = N'Air Jordan 1';
UPDATE dbo.Products SET Price = 11990.00 WHERE ProductName = N'Adidas Ultraboost';
UPDATE dbo.Products SET Price =  9990.00 WHERE ProductName = N'New Balance 574';
UPDATE dbo.Products SET Price =  7490.00 WHERE ProductName = N'Reebok Classic';
GO
