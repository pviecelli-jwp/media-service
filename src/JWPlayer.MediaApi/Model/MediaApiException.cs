namespace JWPlayer.MediaApi.Model;
public class MediaApiException(string message, Exception? innerException = null) : Exception(message, innerException);