## TESTING_NOTES 


1- Total tests = 20 ,  passing = 20  , failing = 0

2- Line Coverage = 90.5% 

3-  I used Mock in Task 2  to mock IOrderRepository, IInventoryService and INotificationService and and used "Verify" to check that (Save & Send) were called Once or Never
    I used AutoFixture in test 3.1 to generate a random Order and customizing each orderItem that Quantity > 0 and UnitPrice >= 0
    I used Bogus in test 3.2 with a fixed seed (123) to generate a realistic customer name and email

4- No, it is not fair . a customer who buys for 999.99 pays 999.99, 
  while a customer who buys for 1,000 gets a 100 discount , so who spends more end up paying less.


5- none 