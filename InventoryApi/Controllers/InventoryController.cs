using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InventoryApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        //Test Data As requirements stated no db to be used for challenge.
        IEnumerable<Inventory> Inventory = new List<Inventory>()
        {
            new Inventory()
                {
                  ModelId= 1,WarehouseId=1, Nomenclature = "Air Intake filter", PartCat = 1, IsReorder = false, ModelName = "K&N", Rdr_Lvl=100, On_Hand=200
                },
            new Inventory()
                {
                  ModelId= 2,WarehouseId=2,Nomenclature = "Valve cover", PartCat = 2, IsReorder = false, ModelName = "Honda", Rdr_Lvl=10, On_Hand=20
                },
            new Inventory()
                {
                  ModelId= 3,WarehouseId=2,Nomenclature = "Wheel bearing rear drivers", PartCat = 3, IsReorder = false, ModelName = "Honda", Rdr_Lvl=20, On_Hand=30
                },
             new Inventory()
                {
                  ModelId= 4, WarehouseId=2,Nomenclature = "Wheel bearing rear passenger", PartCat = 3, IsReorder = false, ModelName = "Ford", Rdr_Lvl=10, On_Hand=20
                },
            new Inventory()
                {
                   ModelId= 5,WarehouseId=1,Nomenclature = "Wheel bearing rear drivers", PartCat = 3, IsReorder = false, ModelName = "Ford", Rdr_Lvl=10, On_Hand=20
                },
            new Inventory()
                {
                   ModelId= 6,WarehouseId=1,Nomenclature = "TMPS sensor", PartCat = 4, IsReorder = true, ModelName = "Universal", Rdr_Lvl=2000, On_Hand=1000
                },
            new Inventory()
                {
                   ModelId= 7,WarehouseId=2,Nomenclature = "5w20 oil 1 qt.", PartCat = 4, IsReorder = true, ModelName = "Universal", Rdr_Lvl=20, On_Hand=100
                }
        };

        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        /**
         * 
         * 
         1) List the quantity of items per warehouse given a particular model ID
         2) List the model name and quantity of items given an optional warehouse identifier of a specified part category
         3) Given a model ID, store ID and optional warehouse ID, reserve an item for a particular store (i.e., take it out of available inventory)
         4) Request an email notification (given an email address) when an item arrives at a store
         5) If familiar, also document the API using the OpenAPI 3.0 (Swagger) standard in either JSON or YAML.
         *
         *
         */

        /// <summary>
        /// Retrieves a specific item by unique id
        /// </summary>
        /// <remarks>Addtl Remarks</remarks>
        /// <response code="200">Product created</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product right now</response>
        [HttpGet("id")]
        [ProducesResponseType(typeof(Inventory), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Inventory>> GetById(int id)
        {
            // Would use FindAsync(id) if we were using a context / db connection.
            var itemById = Inventory.FirstOrDefault(e => e.ModelId == id);
            if (itemById == null)
            {
                return NotFound();
            }
            return itemById;
        }

        /// <summary>
        /// Orders a specific item by unique id
        /// </summary>
        /// <remarks>Addtl Remarks</remarks>
        /// <response code="200">Item list completed.</response>
        /// <response code="400">Item has missing/invalid values.</response>
        /// <response code="500">Cannot get items right now.</response>
        [HttpGet("partCategory")]
        [ProducesResponseType(typeof(Inventory), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<List<Inventory>> GetModelNameAndQtyByPartCat(int partCat)
        {
            var itemsBypartCat = (from n in Inventory.Where(e => e.PartCat == partCat)
                                  select new Inventory()
                                  {
                                      ModelName = n.ModelName,
                                      On_Hand = n.On_Hand
                                  }).ToList();
            if (itemsBypartCat == null)
            {
                throw new Exception("Not Found");
            }
            return itemsBypartCat;
        }

        /// <summary>
        /// Orders a specific item by unique id
        /// </summary>
        /// <remarks>Addtl Remarks</remarks>
        /// <response code="200">Item ordered.</response>
        /// <response code="400">Item has missing/invalid values.</response>
        /// <response code="500">Sorry, Cannot order this item right now.</response>
        [HttpPut("updateOnHand")]
        [ProducesResponseType(typeof(Inventory), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Inventory>> updateQty(int modelID, int storeID, int warehouseID)
        {

            var itemUpdt = Inventory.FirstOrDefault(e => e.ModelId == modelID && e.StoreId == storeID && e.WarehouseId == warehouseID);

            if (itemUpdt == null)
            {
                throw new Exception("Not Found");
            }

            // If on hand is not already 0 then decrement by 1. 
            if (itemUpdt.On_Hand >= 1)
            {
                //Decrement on hand qty and check if reorder threshold has been hit.
                itemUpdt.On_Hand = itemUpdt.On_Hand - 1;
                if (itemUpdt.On_Hand <= itemUpdt.Rdr_Lvl)
                {
                    itemUpdt.IsReorder = true;
                }
            }

            return itemUpdt;
        }

        /// <summary>
        /// Requests Email
        /// </summary>
        /// <remarks>Email to be sent regarding</remarks>
        /// <response code="200">Item ordered.</response>
        /// <response code="400">Item has missing/invalid values.</response>
        /// <response code="500">Sorry, Cannot order this item right now.</response>
        [HttpPut("sendEmail")]
        [ProducesResponseType(typeof(Inventory), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Inventory>> sendEmail(string email, string action, int storeID, int modelId)
        {
            //Not going to create an actual interface for mail delivery here only the accompanying
            //controller that would instantiate an object of that interface.

            var itemUpdt = Inventory.FirstOrDefault(e => e.ModelId == modelId && e.StoreId == storeID);

            if (itemUpdt == null)
            {
                throw new Exception("Not Found");
            }

            //Increment on hand qty and check if reorder threshold has been hit.
            itemUpdt.On_Hand = itemUpdt.On_Hand + 1;
            if (itemUpdt.On_Hand >= itemUpdt.Rdr_Lvl)
            {
                itemUpdt.IsReorder = false;
            }

            return itemUpdt;

        }
    }
}

