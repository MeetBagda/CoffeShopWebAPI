using CoffeeShopWebAPI.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class OrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public IEnumerable<OrderModel> SelectAll()
        {
            var orders = new List<OrderModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrder_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new OrderModel
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        OrderNumber = Convert.ToInt32(reader["OrderNumber"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        PaymentMode = reader["PaymentMode"].ToString(),
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"]) : (decimal?)null,
                        ShippingAddress = reader["ShippingAddress"].ToString(),
                        UserID = Convert.ToInt32(reader["UserID"])
                    });
                }
                return orders;
            }
        }
        #endregion

        #region SelectById
        public OrderModel SelectById(int orderID)
        {
            OrderModel order = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrder_SelectById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    order = new OrderModel
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        OrderNumber = Convert.ToInt32(reader["OrderNumber"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        PaymentMode = reader["PaymentMode"].ToString(),
                        TotalAmount = reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"]) : (decimal?)null,
                        ShippingAddress = reader["ShippingAddress"].ToString(),
                        UserID = Convert.ToInt32(reader["UserID"])
                    };
                }
                return order;
            }
        }
        #endregion

        #region Delete
        public bool Delete(int orderID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrder_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert
        public void Insert(OrderModel order)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrder_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@PaymentMode", order.PaymentMode);
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
                cmd.Parameters.AddWithValue("@UserID", order.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while inserting the order: " + ex.Message, ex);
                }
            }
        }
        #endregion

        #region Update
        public void Update(OrderModel order)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrder_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@OrderID", order.OrderID);
                cmd.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@PaymentMode", order.PaymentMode);
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
                cmd.Parameters.AddWithValue("@UserID", order.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while updating the order: " + ex.Message, ex);
                }
            }
        }
        #endregion

        #region SelectDropdown
        public IEnumerable<OrderDropDownModel> SelectDropdown()
        {
            var ordersDropdown = new List<OrderDropDownModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrder_SelectDropdown", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordersDropdown.Add(new OrderDropDownModel
                        {
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            OrderCode = reader["OrderCode"].ToString()
                        });
                    }

                    return ordersDropdown;
                }
            }
        }
        #endregion
    }
}