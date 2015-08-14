Merona.Pgen.cs
====
pgen packet generator for Merona.cs

Pgen을 이용하면 Merona 서버, 혹은 Merona서버와 통신할 클라이언트에서 사용할 패킷을 쉽게 빌드할 수 있습니다.

![ss](pgen.png)<br>

사용 예제
----
```c#
[PgenTarget]
public class MyGamePackets {
  public class Login {
    [C2S]
    public String id;
    public String password;
    
    [S2C]
    public bool result;
  }
  
  public class ChatMessage {
    public String message;
    
    [S2C]
    public String senderId;
  }
}
```
```c#
static void Main(String[] args){
  P.Gen("packets.h", P.Target.Cpp);
}
```


legacy
----
```c#
public class MyGamePackets
{
  public class Login
  {
    [S2C]
    public string id;
    public string password;
    
    [C2S]
    public bool result;
  }
}
```

```c#
namespace MyGamePackets
{
  public class Login
  {
    public class Request
    {
      public string id;
      public string password;
    }
    public class Response
    {
      public bool result;
    }
  }
}
```

```cpp
namespace MyGamePackets
{
  struct LoginRequest
  {
    std::string id;
    std::string password;
  }
  struct LoginResponse
  {
    bool result;
  }
}
```
