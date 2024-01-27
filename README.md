Integration with external party

Dataset CSV file – this is the input from external system
SQL Server database with only one table – Employees. That table has as many columns as needed to accommodate data from the file.
Simple ASP.Net MVC web site with one page. That page contains 

  Browse File control
  
  a button to execute the import 
  
  a grid/table (described below)
  
When user selects the file and clicks on Import button the program parses the file, gets data and inserts to the database. The page reports on how many rows were successfully processed.
The added rows are shown in the grid on the same page. Data is sorted by surname ascending. Grid supports sorting, searching and edit functionality.
