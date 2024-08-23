using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WebAppProject.Data;
using WebAppProject.Models;
using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

namespace WebAppProject.Controllers;

public class HomeController : Controller
{
    private const int PageSize = 10; // Set your desired page size
    private readonly ApplicationDBContext _customerdb;

    public HomeController(ApplicationDBContext customerdb)
    {
        _customerdb = customerdb;
    }
    public IActionResult Showdata()
    {
            IEnumerable<CustomerCompany> showallCustomerCompany = _customerdb.Customers;
            return View(showallCustomerCompany);
    }
    public IActionResult AddData()
    {
            return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddData(CustomerCompany obj)
    {
        _customerdb.Customers.Add(obj);
        _customerdb.SaveChanges();
        return RedirectToAction("AddData"); /*Unlock Limiter can type on web but must change back after finish "ShowData"*/
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var obj = _customerdb.Customers.Find(id);
        if ( obj ==null)
        {
            return NotFound();
        }
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CustomerCompany obj)
    {   
        if (ModelState.IsValid) 
        { 
        _customerdb.Customers.Update(obj);
        _customerdb.SaveChanges();
        return RedirectToAction("ShowData");
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var obj = _customerdb.Customers.Find(id);
        if (obj == null)
        {
            return NotFound();
        }
        _customerdb.Customers.Remove(obj);
        _customerdb.SaveChanges();
        return RedirectToAction("ShowData");
    }

    public IActionResult ExportExcel()
    {
        using (var workbook = new XLWorkbook()) //Open Excel Editor
        {
            var ws = workbook.Worksheets.Add("Customers"); //Sheet Name
            //Header
            ws.Cell(1,1).Value = "Company Name";
            ws.Cell(1,2).Value = "Branch";
            ws.Cell(1,3).Value = "Nationality";
            ws.Cell(1,4).Value = "Key Person";
            ws.Cell(1,5).Value = "Key Person Phone";
            ws.Cell(1,6).Value = "Website";
            ws.Cell(1,7).Value = "Product/Service";
            ws.Cell(1,8).Value = "Type of Business";
            ws.Cell(1,9).Value = "Number of Employee amounts";
            ws.Cell(1,10).Value = "Fax";
            ws.Cell(1,11).Value = "Authorized person contact phone";
            ws.Cell(1,12).Value = "Email";
            ws.Cell(1,13).Value = "Date of Establishment";
            ws.Cell(1,14).Value = "Address";
            ws.Cell(1,15).Value = "JCC Member ?";
            //Before that connect to SQL first here
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection("Server=LAPTOP-E4O4EFJI\\SQLEXPRESS02; Database=CustomerDB; Trusted_Connection=True; TrustServerCertificate=True");
            SqlDataAdapter ad = new SqlDataAdapter("SELECT * FROM [CustomerDB].[dbo].[Customers]", con);
            ad.Fill(dt);
            int i = 2; //the number what data start row
            foreach(System.Data.DataRow row in dt.Rows)
            {
                //row[0].ToString(); = Id in database column, so [1] to [n] = database column name
                ws.Cell(i, 1).Value = row[1].ToString();
                ws.Cell(i, 2).Value = row[2].ToString();
                ws.Cell(i, 3).Value = row[3].ToString();
                ws.Cell(i, 4).Value = row[4].ToString();
                ws.Cell(i, 5).Value = row[5].ToString();
                ws.Cell(i, 6).Value = row[6].ToString();
                ws.Cell(i, 7).Value = row[7].ToString();
                ws.Cell(i, 8).Value = row[8].ToString();
                ws.Cell(i, 9).Value = row[9].ToString();
                ws.Cell(i, 10).Value = row[10].ToString();
                ws.Cell(i, 11).Value = row[11].ToString();
                ws.Cell(i, 12).Value = row[12].ToString();
                ws.Cell(i, 13).Value = row[13].ToString();
                ws.Cell(i, 14).Value = row[14].ToString();
                ws.Cell(i, 15).Value = row[15].ToString();
                i = i + 1;
            }
            ws.Columns().AdjustToContents();
            var table = ws.Range("A1:O"+(i-1)).CreateTable();
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Customer.CSV"
                    );
            }
            
        }
        
    }

    //public async Task<List<CustomerCompany>> GetAllAsync()
    //{
    //    var customer = await _customerdb.Customers.ToListAsync();
    //    var customercompany = new CustomerCompany
    //    {
    //        Id = customer.Id,
    //        Company_Name = customer.Company_Name,
    //        Branch = customer.Branch,
    //        Nationality = customer.Nationality,
    //        Key_Person = customer.Key_Person,
    //        Key_Person_Phone = customer.Key_Person_Phone,
    //        Website = customer.Website,
    //        ProductService = customer.ProductService,
    //        Type_of_Business = customer.Type_of_Business,
    //        Number_of_employees = customer.Number_of_employees,
    //        Fax = customer.Fax,
    //        Authorized_person_contract_Phone = customer.Authorized_person_contract_Phone,
    //        Email = customer.Email,
    //        Date_of_Establishment = customer.Date_of_Establishment,
    //        Address = customer.Address,
    //    };
    //    return customercompany;
    //}


}
