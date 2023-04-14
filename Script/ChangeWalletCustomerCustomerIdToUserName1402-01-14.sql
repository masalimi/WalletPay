--ALTER TABLE WalletCustomer 
--ALTER COLUMN CustomerId nvarchar(1000);

--EXEC sp_rename 'WalletCustomer.CustomerId', 'Username', 'COLUMN';
--ALTER TABLE WalletCustomer DROP COLUMN IsChange;


--ALTER TABLE WalletCustomerAmount 
--ALTER COLUMN Amount decimal(18,4);