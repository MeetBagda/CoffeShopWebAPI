using CoffeeShopWebAPI.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class OrderDetailsRepository
    {
        private readonly string _connectionString;

        public OrderDetailsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public IEnumerable<OrderDetailsModel> SelectAll()
        {
            var orderDetailsList = new List<OrderDetailsModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrderDetails_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    orderDetailsList.Add(new OrderDetailsModel
                    {
                        OrderDetailID = Convert.ToInt32(reader["OrderDetailID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        ProductID = reader["ProductID"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        UserID = Convert.ToInt32(reader["UserID"])
                    });
                }
                return orderDetailsList;
            }
        }
        #endregion

        #region SelectById
        public OrderDetailsModel SelectById(int orderDetailID)
        {
            OrderDetailsModel orderDetail = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrderDetails_SelectById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderDetailID", orderDetailID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    orderDetail = new OrderDetailsModel
                    {
                        OrderDetailID = Convert.ToInt32(reader["OrderDetailID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        ProductID = reader["ProductID"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        UserID = Convert.ToInt32(reader["UserID"])
                    };
                }
                return orderDetail;
            }
        }
        #endregion

        #region Delete
        public bool Delete(int orderDetailID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrderDetails_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@OrderDetailID", orderDetailID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert
        public void Insert(OrderDetailsModel orderDetail)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrderDetails_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                cmd.Parameters.AddWithValue("@Amount", orderDetail.Amount);
                cmd.Parameters.AddWithValue("@TotalAmount", orderDetail.TotalAmount);
                cmd.Parameters.AddWithValue("@UserID", orderDetail.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while inserting the order detail: " + ex.Message, ex);
                }
            }
        }
        #endregion

        #region Update
        public void Update(OrderDetailsModel orderDetail)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spOrderDetails_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@OrderDetailID", orderDetail.OrderDetailID);
                cmd.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                cmd.Parameters.AddWithValue("@Amount", orderDetail.Amount);
                cmd.Parameters.AddWithValue("@TotalAmount", orderDetail.TotalAmount);
                cmd.Parameters.AddWithValue("@UserID", orderDetail.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while updating the order detail: " + ex.Message, ex);
                }
            }
        }
        #endregion

        //#region SelectDropdown
        //public IEnumerable<OrderDropDownModel> SelectDropdown()
        //{
        //    var ordersDropdown = new List<OrderDropDownModel>();
        //    using (SqlConnection conn = new SqlConnection(_connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("spOrderDetails_SelectDropdown", conn)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        conn.Open();

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                ordersDropdown.Add(new OrderDropDownModel
        //                {
        //                    OrderDetailID = Convert.ToInt32(reader["OrderDetailID"]),
        //                    ProductID = reader["ProductName"].ToString() // Assuming ProductName is available in the dropdown.
        //                });
        //            }

        //            return ordersDropdown;
        //        }
        //    }
        //}
        //#endregion
    }
}