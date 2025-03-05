using CoffeeShopWebAPI.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace CoffeeShopWebAPI.Data
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public IEnumerable<CustomerModel> SelectAll()
        {
            var customers = new List<CustomerModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomer_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new CustomerModel
                    {
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        CustomerName = reader["CustomerName"].ToString(),
                        HomeAddress = reader["HomeAddress"].ToString(),
                        Email = reader["Email"].ToString(),
                        MobileNo = reader["MobileNo"].ToString(),
                        GSTNo = reader["GSTNo"].ToString(),
                        CityName = reader["CityName"].ToString(),
                        PinCode = reader["PinCode"].ToString(),
                        NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                        UserID = Convert.ToInt32(reader["UserID"])
                    });
                }
                return customers;
            }
        }
        #endregion

        #region SelectById
        public CustomerModel SelectById(int customerID)
        {
            CustomerModel customer = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomer_SelectById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    customer = new CustomerModel
                    {
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        CustomerName = reader["CustomerName"].ToString(),
                        HomeAddress = reader["HomeAddress"].ToString(),
                        Email = reader["Email"].ToString(),
                        MobileNo = reader["MobileNo"].ToString(),
                        GSTNo = reader["GSTNo"].ToString(),
                        CityName = reader["CityName"].ToString(),
                        PinCode = reader["PinCode"].ToString(),
                        NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                        UserID = Convert.ToInt32(reader["UserID"])
                    };
                }
                return customer;
            }
        }
        #endregion

        #region Delete
        public bool Delete(int customerID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomer_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Insert
        public void Insert(CustomerModel customer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomer_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@HomeAddress", customer.HomeAddress);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
                cmd.Parameters.AddWithValue("@GSTNo", customer.GSTNo);
                cmd.Parameters.AddWithValue("@CityName", customer.CityName);
                cmd.Parameters.AddWithValue("@PinCode", customer.PinCode);
                cmd.Parameters.AddWithValue("@NetAmount", customer.NetAmount);
                cmd.Parameters.AddWithValue("@UserID", customer.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while inserting the customer: " + ex.Message, ex);
                }
            }
        }
        #endregion

        #region Update
        public void Update(CustomerModel customer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spCustomer_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@HomeAddress", customer.HomeAddress);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
                cmd.Parameters.AddWithValue("@GSTNo", customer.GSTNo);
                cmd.Parameters.AddWithValue("@CityName", customer.CityName);
                cmd.Parameters.AddWithValue("@PinCode", customer.PinCode);
                cmd.Parameters.AddWithValue("@NetAmount", customer.NetAmount);
                cmd.Parameters.AddWithValue("@UserID", customer.UserID);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while updating the customer: " + ex.Message, ex);
                }
            }
        }
        #endregion

        #region SelectDropdown
        public IEnumerable<CustomerDropDownModel> SelectDropdown()
        {
            var customersDropdown = new List<CustomerDropDownModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUsers_SelectDropdown", conn) // Assuming this is the correct stored procedure for dropdown.
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customersDropdown.Add(new CustomerDropDownModel
                        {
                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                            CustomerName = reader["CustomerName"].ToString()
                        });
                    }

                    return customersDropdown;
                }
            }
        }
        #endregion
    }
}