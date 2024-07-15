namespace FasceMVC.Controllers
{
    internal class TotalGeneral
    {
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Cargos { get; set; }
        public decimal Descuentos { get; set; }
        public decimal SubTotalSinCargosDescuentos { get; set; }
        public decimal IVA { get; set; }
        public decimal TotalRetencion { get; set; }
    }

}