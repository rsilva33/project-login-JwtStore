namespace JwtStore.Core.Contexts.AccountContext.ValueObjects;

public partial class Email : ValueObject
{
  private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

  protected Email() { }

  public Email(string address)
  {
    if(string.IsNullOrEmpty(address))
      throw new Exception("Invalid e-mail");

    Address = address.Trim().ToLower();

    if(Address.Length < 5)
      throw new Exception("Invalid e-mail");

    if(!EmailRegex().IsMatch(Address))
      throw new Exception("Invalid e-mail");
  }

  public string Address { get; }
  public Verification Verification { get; private set; } = new();

  public void ResendVerification()
    => Verification = new Verification();

  public string Hash
    => Address.ToBase64();//Gravatar.com

  //Convert email to string
  public static implicit operator string(Email email) 
    => email.ToString();
  
  //Convert string to email 
  public static implicit operator Email(string address)
    => new(address);

  //Convert 
  public override string ToString()
    => Address;
    
  [GeneratedRegex(Pattern)]
  private static partial Regex EmailRegex();
}