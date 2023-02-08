using Whatsapp.Model;

namespace Whatsapp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using (IWhatsappService whatsAppService = new WhatsappService(new WhatsappDbContext()))
            {
                var userData = new UserViewModel
                {
                    UserName = "Eze",
                    PhoneNumber = "08072688333",
                };

                //INSERT 
                //var createdUserId = await whatsAppService.CreateUser(userData);
                //Console.WriteLine(createdUserId);


                //UPDATE
                //var updatedUser = await whatsAppService.UpdateUser(10, userData);
                //Console.WriteLine(updatedUser ? $"Updated Successfully!" : $"Update not Successful!");


                //DELETE
                //var deletedUser = await whatsAppService.DeleteUser(11);
                //Console.WriteLine(deletedUser ? $"Successfully Deleted!" : $"Not Successfully Deleted!");


                //GET USER
                //var user = await whatsAppService.GetUser(4);
                //Console.WriteLine($"UserName : {user.UserName} \t Phone Number : {user.PhoneNumber}");


                //GET ALL USERS
                var allUsers = await whatsAppService.GetUsers();
                foreach (var user in allUsers)
                {
                    Console.WriteLine($"UserName : {user.UserName}\t Phone Number : {user.PhoneNumber}");
                }



            }
        }

    }
}