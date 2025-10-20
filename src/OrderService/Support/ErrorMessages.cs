namespace OrderService.Support
{
    public static class ErrorMessages
    {
        public const string OrderAlreadyExists = "Order with same Id is already created";
        public const string MissingCustomerName = "Customer Name must be present";
        public const string EmptyItems = "No items found in the order";
        public const string IncorrectQuantityValues = "There are items with incorrect Quantity values";
        public const string DuplicateProducts = "Same ProductId appeared multiple times in item list";
    }
}
