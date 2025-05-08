namespace gremlin_eye.Server.Interfaces
{
    public interface IChecksum //used to detect differences
    {
        //if checksum on igdb side is different, then that means the data in igdb has been changed and needs to be updated
        string? Checksum { get; set; }
    }
}
