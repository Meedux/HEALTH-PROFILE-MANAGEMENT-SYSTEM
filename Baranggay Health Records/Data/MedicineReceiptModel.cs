using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class MedicineReceiptModel
    {
        public int ID { get; set; }

        public int residentId { get; set; }

        public int medicineId { get; set; }

        public int quantity { get; set; }        

        public int GetID()
        {
            return ID;
        }

        public int GetResidentID()
        {
            return this.residentId;
        }

        public int GetQuantity()
        {
            return this.quantity;
        }

        public int GetMedicineID()
        {
            return this.medicineId;
        }
    }
}
