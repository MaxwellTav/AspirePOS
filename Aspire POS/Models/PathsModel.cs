namespace Aspire_POS.Models
{
    public class PathsModel
    {
        /// <summary>
        /// Endpoint de autenticación de WooCommerce y WordPress, utilizado para obtener un token JWT (JSON Web Token) basado en credenciales de usuario.
        /// </summary>
        public const string GETTOKEN = "wp-json/jwt-auth/v1/token";

        /// <summary>
        /// Endpoint de WooCommerce que permite gestionar los pedidos (orders) de una tienda en línea a través de la API REST de WooCommerce.
        /// </summary>
        public const string ORDERS = "wp-json/wc/v3/orders";

        /// <summary>
        /// Obtiene todos los usuarios registrados en WordPress
        /// </summary>
        public const string USERS = "wp-json/wp/v2/users";

        /// <summary>
        /// Obtiene todos los productos registrados en WooComerce
        /// </summary>
        public const string PRODUCTS = "wp-json/wc/v3/products";
    }
}
