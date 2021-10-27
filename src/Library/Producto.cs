using System.Collections;

namespace ClassLibrary
{
    /// <summary>
    /// Representa el producto de una oferta
    /// </summary>
    public class Product
    {
        private Classification classification;
        private double quantity;
        private double price;
        private string ubication;

/// <summary>
/// Obtiene un obtiene una instancia de classificacion valida para un producto.
/// </summary>
/// <value></value>
        public Classification Classification
        {
            get
            {
                return this.classification;
            }
            set
            {
                this.classification = value;
            }
        }
/// <summary>
/// Determina la cantidad de unidades de un producto.
/// </summary>
/// <value></value>
        public double Quantity
        {
            get
            {
                return this.quantity;
            }
            set
            {
                this.quantity = value;
            }
        }
/// <summary>
/// Determina el precio por unidad de producto
/// </summary>
/// <value></value>
        public double Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
            }
        }
/// <summary>
/// Determina la ubicacion de un producto.
/// </summary>
/// <value></value>
        public string Ubication
        {
            get
            {
                return this.ubication;
            }
            set
            {
                this.ubication = value;
            }
        }
/// <summary>
/// Constructor de instancias de product.
/// </summary>
/// <param name="classification"></param>
/// <param name="quantity"></param>
/// <param name="price"></param>
/// <param name="ubication"></param>
        public Product(Classification classification, double quantity, double price, string ubication)
        {
            this.Classification = classification;
            this.Quantity = quantity;
            this.Price = price;
            this.Ubication = ubication;
        }


    }
}