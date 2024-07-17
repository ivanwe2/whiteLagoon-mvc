using Microsoft.AspNetCore.Mvc;

namespace WhiteLagoon.Web.Extensions
{
    public static class ControllerExtensionMethods
    {
        public static void AddSuccessMessageToTempData(this Controller controller,string message)
        {
            
            controller.TempData["success"] = message;
        }
        public static void AddErrorMessageToTempData(this Controller controller, string message)
        {
            controller.TempData["error"] = message;
        }
    }
}
