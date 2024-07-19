﻿using Microsoft.AspNetCore.Mvc;
using SalesWebMVc.Models;
using SalesWebMVc.Models.ViewModels;
using SalesWebMVc.Services;
using SalesWebMVc.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMVc.Controllers
{
	public class SellersController : Controller
	{
		//The Sellers Controller makes the interface between the View and the Model
		//MVC pattern!!
		//receives the data from the view and sends it to the model
		private readonly SellerService _sellerService;
		private readonly DepartmentService _departmentService;

		public SellersController(SellerService sellerService, DepartmentService departmentService)
		{
			_sellerService = sellerService;
			_departmentService = departmentService;
		}
		public IActionResult Index()
		{
			var list = _sellerService.FindAll();
			//Returning the list to the view in the form of a model
			return View(list);
		}
		public IActionResult Create()
		{
			var departments = _departmentService.FindAll();
			var viewModel = new SellerFormViewModel { Departments = departments };
			//this create returns the view with the form to insert the data
			return View(viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Seller seller ) 
		{
			if (!ModelState.IsValid)
			{
				var departments = _departmentService.FindAll();
				var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
				return View(viewModel);
			}
			//this create is to insert the data
			_sellerService.Insert(seller);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}
			return View(obj);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			_sellerService.Remove(id);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}
			return View(obj);
		}
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}
			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not found" });
			}
			List<Department> departments = _departmentService.FindAll();
			SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
			return View(viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, Seller seller)
		{
			if (!ModelState.IsValid)
			{
				var departments = _departmentService.FindAll();
				var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
				return View(viewModel);
			}
			if (id != seller.Id)
			{
				return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
			}
			try
			{
				_sellerService.Update(seller);
				return RedirectToAction(nameof(Index));
			}
			catch (NotFoundException e)
			{
				return RedirectToAction(nameof(Error), new { message = e.Message });
			}
			catch (DbConcurrencyException e)
			{
				return RedirectToAction(nameof(Error), new { message = e.Message });
			}
		}

		public IActionResult Error(string message)
		{
            var viewModel = new ErrorViewModel
			{
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
	}
}
