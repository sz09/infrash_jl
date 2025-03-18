namespace JobLogic.Infrastructure.ServiceResponders
{
    public enum ResponseStatus
    {
        //When it all goes ok
        SUCCESS,

        //BUSINESS LOGIC ERROR, FOR EXAMPLE TRYING TO CREATE A JOB FOR AN INACTIVE CUSTOMER
        FAILURE,

        //When a system exceptoin occurs, eg db failure, disk error, etc
        EXCEPTION,

        //Validation Error on data input
        VALIDATION,

        PERMISSION_DENIED
    }
}
