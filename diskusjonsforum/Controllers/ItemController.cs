//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using diskusjonsforum.Models;
//namespace diskusjonsforum.Controllers

//{
//	public class ItemController : Controller
//	{
//		private readonly ItemDbContext _itemDbContext;

//        public ItemController(ItemDbContext _itemDbContext)
//		{
//			db = _itemDbContext;
//		}

//		public IActionResult Table()
//		{
//			List<Item> items = _itemDbContext.Items.ToList();
//			var listItemViewModel = new ListItemViewModel(items, "Table");
//			return View;
//		}
//	}
//}

