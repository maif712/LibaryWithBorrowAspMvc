using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace LibaryWithBorrowAspMvc.Extension
{
    public static class ControllerToastExtensions
    {
        public static void AddToast(this Controller controller, string message, string type)
        {
            controller.TempData[StaticMessage.TOAST_MESSAGE] = message;
            controller.TempData[StaticMessage.TOAST_TYPE] = type;
        }

        public static void AddError(this Controller controller, string message) =>
            controller.AddToast(message, StaticMessage.ERROR);

        public static void AddSuccess(this Controller controller, string message) =>
            controller.AddToast(message, StaticMessage.SUCCESS);

        public static void AddWarning(this Controller controller, string message) =>
            controller.AddToast(message, StaticMessage.WARNING);

        public static void AddInfo(this Controller controller, string message) =>
            controller.AddToast(message, StaticMessage.INFO);
    }

}
