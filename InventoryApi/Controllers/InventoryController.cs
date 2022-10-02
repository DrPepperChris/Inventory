using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text.Json.Nodes;

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
                  ModelId= 1,WarehouseId=1, Nomenclature = "Air Intake filter", PartCat = 1, IsReorder = false, ModelName = "K&N", Rdr_Lvl=100, On_Hand=0, StoreId=1, 
                },
            new Inventory()
                {
                  ModelId= 2,WarehouseId=2,Nomenclature = "Valve cover", PartCat = 2, IsReorder = false, ModelName = "Honda", Rdr_Lvl=10, On_Hand=20, StoreId=1,
                },
            new Inventory()
                {
                  ModelId= 3,WarehouseId=2,Nomenclature = "Wheel bearing rear drivers", PartCat = 3, IsReorder = false, ModelName = "Honda", Rdr_Lvl=20, On_Hand=30, StoreId=1,
                },
             new Inventory()
                {
                  ModelId= 4, WarehouseId=2,Nomenclature = "Wheel bearing rear passenger", PartCat = 3, IsReorder = false, ModelName = "Ford", Rdr_Lvl=10, On_Hand=20, StoreId=1,
                },
            new Inventory()
                {
                   ModelId= 5,WarehouseId=1,Nomenclature = "Wheel bearing rear drivers", PartCat = 3, IsReorder = false, ModelName = "Ford", Rdr_Lvl=10, On_Hand=20, StoreId=1,
                },
            new Inventory()
                {
                   ModelId= 6,WarehouseId=1,Nomenclature = "TMPS sensor", PartCat = 4, IsReorder = true, ModelName = "Universal", Rdr_Lvl=2000, On_Hand=1000, StoreId=1,
                },
            new Inventory()
                {
                   ModelId= 7,WarehouseId=2,Nomenclature = "5w20 oil 1 qt.", PartCat = 4, IsReorder = true, ModelName = "Universal", Rdr_Lvl=20, On_Hand=100, StoreId=1,
                },

             new Inventory()
                {
                  ModelId= 8,WarehouseId=1, Nomenclature = "Air Intake filter", PartCat = 1, IsReorder = false, ModelName = "K&N", Rdr_Lvl=100, On_Hand=200, StoreId=1,
                },
            new Inventory()
                {
                  ModelId= 9,WarehouseId=2,Nomenclature = "Valve cover", PartCat = 2, IsReorder = false, ModelName = "Honda", Rdr_Lvl=10, On_Hand=20, StoreId=1,
                },
            new Inventory()
                {
                  ModelId= 10,WarehouseId=2,Nomenclature = "Wheel bearing rear drivers", PartCat = 3, IsReorder = false, ModelName = "Honda", Rdr_Lvl=20, On_Hand=30, StoreId=1,
                },
             new Inventory()
                {
                  ModelId= 11, WarehouseId=2,Nomenclature = "Wheel bearing rear passenger", PartCat = 3, IsReorder = false, ModelName = "Ford", Rdr_Lvl=10, On_Hand=20, StoreId=1,
                },
            new Inventory()
                {
                   ModelId= 12,WarehouseId=1,Nomenclature = "Wheel bearing rear drivers", PartCat = 3, IsReorder = false, ModelName = "Ford", Rdr_Lvl=10, On_Hand=20, StoreId=1,
                },
            new Inventory()
                {
                   ModelId= 13,WarehouseId=1,Nomenclature = "TMPS sensor", PartCat = 4, IsReorder = true, ModelName = "Sensor corp", Rdr_Lvl=2000, On_Hand=1000, StoreId=1,
                },
            new Inventory()
                {
                   ModelId= 14,WarehouseId=2,Nomenclature = "5w20 oil 1 qt.", PartCat = 4, IsReorder = true, ModelName = "Oil Co", Rdr_Lvl=20, On_Hand=100, StoreId=1,
                }
        };
        
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }      

        /// <summary>
        /// Retrieves a specific item by unique id
        /// </summary>
        /// <remarks>Retrieves items by ID and uses optional warehouseId param</remarks>
        /// <response code="200">Product list retrieved</response>
        /// <response code="400">Product has missing/invalid item id</response>
        /// <response code="500">Oops! Can't retrieve your product right now</response>
        [HttpGet("GetbyId")]
        [ProducesResponseType(typeof(Inventory), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Inventory>> GetById(int id, int warehouseId)
        {
            var itemById = Inventory.FirstOrDefault(e => e.ModelId == id && e.WarehouseId == warehouseId);
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
        [HttpGet("partByCategory/{partCat}")]
        [ProducesResponseType(typeof(Inventory), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<List<Inventory>> GetModelNameAndQtyByPartCat(int partCat, int? WarehouseId)
        {

            var itemsBypartCat = (from n in Inventory.Where(e => e.PartCat == partCat &&
                                 (WarehouseId == null || WarehouseId == e.WarehouseId))
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
        public async Task<ActionResult<Inventory>> updateQty(int modelID, int? storeID, int? warehouseID)
        {

            var itemUpdt = Inventory.FirstOrDefault(e => e.ModelId == modelID && (storeID == null || e.StoreId == storeID)
            && (warehouseID == null || warehouseID == e.WarehouseId));

            if (itemUpdt == null)
            {
                throw new Exception("Not Found");
            }
            else
            {
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
                if (itemUpdt.On_Hand == 0)
                {
                    return NotFound("Quantity on hand is currently 0. Please order more of " + itemUpdt.Nomenclature + ", and try again.");
                }
            }
            return itemUpdt;
        }

        /// <summary>
        /// Requests Email
        /// </summary>
        /// <remarks>Email to be sent including itemId</remarks>
        /// <response code="200">Item ordered.</response>
        /// <response code="400">email is required.</response>
        /// <response code="500">Sorry Cannot send email right now.</response>
        [HttpPut("sendEmail")]
        [ProducesResponseType(typeof(Inventory), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public ActionResult sendEmail(string? userId, string UserEmail,int modelID)
        {
            try
            {
                var senderEmail = new MailAddress("CompanyEmail@gmail.com", "CompanyEmail");
                var receiverEmail = new MailAddress(UserEmail, "Receiver");
                var password = "pwd123";
                var sub = "Confirmation of Item Order";
                var body = "Message Body here. You bought " + modelID + ", it will arrive soon";  //omitted for time reasons.
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",  // or whatever smtp server / host is used.
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = "Confirmation of Item Order",
                    Body = body
                })
                {
                    return Ok(mess);  // smtp.Send(mess); replace this with actual smtp credentials.
                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                var text = "error message: " + ex.ToString();
                return BadRequest(text);
            }
        }
    }
}

