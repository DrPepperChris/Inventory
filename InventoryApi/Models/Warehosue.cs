using System.ComponentModel.DataAnnotations;

namespace InventoryApi
{
    /**
     * 
        For nomenclature, a model is a specific kind of part (Manufacturer/Model Number), 
        while an item is a physical part in inventory that is identified by model.
        Models are child elements of a “part category”, e.g. transmission, brakes, engine, etc.
     * 
     */

    public class Warehouse
    {
        // Items are stored here, used to refill local stores
        public int WarehouseId { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }

    }
}