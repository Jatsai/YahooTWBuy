/* Copyright (C) Ion OÜ - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Mihhail Maslakov <mihhail.maslakov@gmail.com>, 2017
 */
#pragma warning disable
namespace __livexaml { 
using System;using System.Collections.Generic;using System.Diagnostics;using
System.Linq;using System.Reflection;using System.Text;using System.Threading.
Tasks;using Xamarin.Forms;using System.Collections;using System.Collections.
Specialized;using System.Xml.Linq;using Xamarin.Forms.Internals;using Xamarin.
Forms.Xaml;using Xamarin.Forms.Xaml.Internals;using System.ComponentModel;using
System.Collections.ObjectModel;using System.Threading;using System.IO;using
System.Xml;internal class K{private string[]o;private int p;public K(string[]a){
if(a.Length==0)throw new InvalidOperationException(
"Can't initialize circular buffer with an empty collection");o=a;}public string
Get(){return o[p++%o.Length];}}internal class L{public static void WriteLine(
string a,int b=0){
#if !LIVEXAML_DEBUG
if(b==1)return;
#endif
Debug.
WriteLine("XL: "+a);}public static void Exception(Exception a,int b=0){if(a is
TargetInvocationException){var c=(TargetInvocationException)a;WriteLine(c.
InnerException.Message,b);}else{WriteLine(a.Message,b);}}internal static void o(
string a){WriteLine(a,0);}internal static void p(string a){WriteLine(
"=============== LiveXAML Important Information ===============",0);WriteLine(a,
0);WriteLine("==============================================================",0)
;}internal static void q(string a){WriteLine(a,1);}}internal class N{static
FieldInfo o=typeof(ResourceDictionary).GetTypeInfo().GetDeclaredField(
"_mergedInstance");public static void CreateNewMergedResourceDictionary(
ResourceDictionary a){if(a==null){L.WriteLine(
"Can't update merged resource dictionary on a null target");return;}o.SetValue(a
,Activator.CreateInstance(a.MergedWith));}public static bool
GetMergedResourceDictionary(Element a,out ResourceDictionary b){b=null;var c=a
as VisualElement;if(c!=null)return GetMergedResourceDictionary(c.Resources,out b
);var d=a as Application;if(d!=null)return GetMergedResourceDictionary(d.
Resources,out b);return false;}public static bool GetMergedResourceDictionary(
ResourceDictionary a,out ResourceDictionary b){b=null;if(a!=null){var c=o.
GetValue(a)as ResourceDictionary;if(c!=null){b=c;return true;}}return false;}}
internal class O{private static Dictionary<Type,string>o=new Dictionary<Type,
string>();private static Dictionary<string,string>p=new Dictionary<string,string
>();private static Func<Type,string>q;private static PropertyInfo r=typeof(
XamlLoader).GetRuntimeProperty("XamlFileProvider");private static bool s;private
static int t;public static bool InitializeComponent(Element a,string b=null){try
{L.WriteLine("InitializeComponent "+a);if(a is Application){if(!string.
IsNullOrWhiteSpace(b))G(a,b);var c=Application.Current.MainPage;if(c is
NavigationPage){InitializeComponent(((NavigationPage)c).CurrentPage);}else{
InitializeComponent(c);}}else{var d=a.GetType().GetAllFields().ToArray();C(a,d);
if(!x(a,b)){L.WriteLine("Constructor initializing failed.");return false;}D(a,d)
;E(a);H(a);}return true;}catch(TargetInvocationException e){I(a,e.InnerException
);}catch(Exception e){I(a,e);}return false;}internal static void u(Type a,string
b){o[a]=b;}internal static void u(string a,string b){p[a]=b;}public static void
SetElementXamlFileProvider(){if(!s){s=true;v(w);}}private static void v(Func<
Type,string>a){q=XamlLoader.XamlFileProvider;r.SetValue(null,a);}private static
string w(Type a){if(t-->0){if(q!=null)q(a);return null;}string b;if(o.
TryGetValue(a,out b))return b;if(p.TryGetValue(a.FullName,out b))return b;if(q==
null)return null;return q(a);}private static bool x(Element a,string b){try{var
c=a.GetType();var d=c.FindConstructor();if(d==null)return false;t=c.
GetCustomControlAncestorCount();var f=(Element)Activator.CreateInstance(c);f.
Parent=a.Parent;if(!string.IsNullOrWhiteSpace(b))G(a,b);y(a,c,f);z(a,f);B(a,f);
try{if(f is BindableObject&&a is BindableObject){var g=typeof(BindableObject).
GetTypeInfo().DeclaredFields.FirstOrDefault(h=>h.Name=="_properties");var i=(
BindableObject)f;if(g!=null){var j=g.GetValue(a)as IEnumerable;foreach(var
propertyValue in j){var k=propertyValue.GetFieldValue("Property")as
BindableProperty;if(k.PropertyName=="Register"&&k.DeclaringType.FullName==
"__livexaml.Runtime")continue;var l=propertyValue.GetFieldValue("Value");try{i.
CallMethod("SetValue",k,l,false,false);}catch(Exception e){L.q(
"Failed to SetValue for property "+k.PropertyName);L.q(e.ToString());if(k!=null
&&!k.IsReadOnly)i.SetValue(k,l);}}}}}catch(Exception e){L.o(
"Copying BindableProperties failed: "+e.Message);}return true;}catch(Exception e
){L.WriteLine("Failed to call ctor: "+e.Message);return false;}}private static
void y(Element a,Type b,Element c){var d=b.GetRuntimeFields();foreach(var fld in
d){try{if(!fld.IsLiteral&&!fld.IsStatic){var f=fld.GetValue(c);if(f!=null)fld.
SetValue(a,f);}}catch(Exception){L.WriteLine("Couldn't copy field "+fld.Name);}}
}private static void z(Element a,Element b){var c=a.GetType().GetTypeInfo();var
d=new List<TypeInfo>();d.Add(c);while(c.BaseType!=null){c=c.BaseType.GetTypeInfo
();d.Add(c);}var f=new HashSet<string>();for(int g=d.Count-1;g>=0;g--){var h=d[g
].AsType();var i=J(h);if(i==null)continue;if(f.Contains(i))continue;f.Add(i);var
j=h.GetRuntimeProperties().FirstOrDefault(k=>k.Name==i);var l=j.GetValue(a);z(a,
b,h,j,l);}}private static void z(Element a,Element b,Type c,PropertyInfo d,
object f){if(a is Grid){A((Grid)b,(Grid)a);}else if(f is System.Collections.
IEnumerable){var g=f.GetType().GetRuntimeMethods();var h=g.FirstOrDefault(i=>i.
Name=="Clear");var j=g.FirstOrDefault(i=>i.Name=="Add");if(h==null||j==null)
return;var k=d.GetValue(b)as System.Collections.IEnumerable;if(k==null)return;h.
Invoke(f,new object[0]);foreach(var newValue in k.OfType<object>().ToList())j.
Invoke(f,new object[]{newValue});var l=c.GetAllDeclaredMethods().ToList();var m=
l.FirstOrDefault(i=>i.Name=="OnChildrenChanged");if(m!=null){m.Invoke(a,new
object[]{a,new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.
Reset)});}var n=l.FirstOrDefault(i=>i.Name=="OnPagesChanged");if(n!=null){n.
Invoke(a,new object[]{new NotifyCollectionChangedEventArgs(
NotifyCollectionChangedAction.Reset)});}}else{d.SetValue(a,d.GetValue(b));}}
private static void A(Grid a,Grid b){var c=a.Children.ToArray();b.Children.Clear
();a.Children.Clear();foreach(var child in c){var d=Grid.GetRow(child);var f=
Grid.GetRowSpan(child);var g=Grid.GetColumn(child);var h=Grid.GetColumnSpan(
child);b.Children.Add(child,g,g+h,d,d+f);}}private static void B(object a,object
b){if(a is Page&&b is Page){((Page)a).ToolbarItems.Clear();foreach(var newItem
in((Page)b).ToolbarItems)((Page)a).ToolbarItems.Add(newItem);}}private static
void C(object a,FieldInfo[]b){var c=a.GetType();var d=b.FirstOrDefault(f=>f.Name
=="Disappearing");var g=c.GetTypeInfo().DeclaredMethods.FirstOrDefault(h=>h.Name
=="OnDisappearing"&&h.GetParameters().Length==0);if(g!=null)g.Invoke(a,null);if(
d!=null){var i=(EventHandler)d.GetValue(a);if(i!=null)i.Invoke(a,EventArgs.Empty
);}}private static void D(object a,FieldInfo[]b){var c=a.GetType();var d=b.
FirstOrDefault(f=>f.Name=="Appearing");var g=c.GetTypeInfo().DeclaredMethods.
FirstOrDefault(h=>h.Name=="OnAppearing"&&h.GetParameters().Length==0);if(g!=null
)g.Invoke(a,null);if(d!=null){var i=(EventHandler)d.GetValue(a);if(i!=null)i.
Invoke(a,EventArgs.Empty);}}private static void E(object a){try{var b=a as
Xamarin.Forms.BindableObject;if(b==null)return;var c=b.BindingContext;if(c==null
)return;b.BindingContext=null;b.BindingContext=c;}catch(Exception e){L.WriteLine
("Failed to refresh BindingContext: "+e.Message);}}private static void F(object
a,IReadOnlyList<string>b){var c=typeof(NameScopeExtensions).GetRuntimeMethod(
"FindByName",new[]{typeof(Element),typeof(string)});var d=a.GetType();var f=d.
GetRuntimeFields();foreach(var xname in b){try{var g=f.FirstOrDefault(h=>h.Name
==xname);if(g!=null){var i=c.MakeGenericMethod(g.FieldType);var j=i.Invoke(null,
new object[]{a,xname});g.SetValue(a,j);}else{L.WriteLine("Couldn't find field "+
xname+" on type "+d.Name);}}catch(Exception e){L.WriteLine(
"Failed to assign named control "+xname);L.WriteLine(e.ToString());}}}private
static void G(object a,string b){var c=typeof(Xamarin.Forms.Xaml.Extensions).
GetTypeInfo();var d=c.DeclaredMethods.FirstOrDefault(f=>{if(f.Name!=
"LoadFromXaml")return false;var g=f.GetParameters();return g.Length==2&&g[1].
ParameterType==typeof(string);});if(d==null)throw new InvalidOperationException(
"Couldn't find LoadFromXaml extension method");var h=d.MakeGenericMethod(typeof(
object));h.Invoke(null,new[]{a,b});}private static void H(object a){var b=a.
GetType().GetRuntimeMethods().Where(c=>c.Name.EndsWith("LiveXaml",
StringComparison.OrdinalIgnoreCase));foreach(var initMethod in b){if(initMethod.
GetParameters().Length>0)L.WriteLine(
"LiveXaml init method cannot contain any parameters ("+initMethod.Name+")");else
initMethod.Invoke(a,null);}}private static void I(Element a,Exception b){var c=b
.Message;var d=b.InnerException;while(d!=null){c+=Environment.NewLine+d.Message;
d=d.InnerException;}var f=new Label{Text=c,TextColor=Color.Red};if(a is
ContentPage){((ContentPage)a).Content=f;}else if(a is ContentView){((ContentView
)a).Content=f;}L.WriteLine(c);}private static string J(Type a){TypeInfo b=a.
GetTypeInfo();var c=b.CustomAttributes.FirstOrDefault(d=>d.AttributeType==typeof
(ContentPropertyAttribute));if(c!=null&&c.ConstructorArguments.Count>0){var f=c.
ConstructorArguments[0];if(f.ArgumentType==typeof(string))return(string)f.Value;
}if(b.BaseType!=null)return J(b.BaseType);return null;}}internal static class P{
private static readonly Dictionary<string,Type>o=new Dictionary<string,Type>();
private static readonly Assembly[]p;static P(){try{p=s();}catch{L.o(
"Failed to load assemblies");p=new Assembly[]{typeof(Element).GetTypeInfo().
Assembly,typeof(string).GetTypeInfo().Assembly};}}public static Tuple<Type,
FieldInfo>FindShortTypeWithProperty(string a,string b){q();foreach(var kvp in o)
{if(kvp.Key.EndsWith("."+a)){var c=FindBindablePropertyField(kvp.Value,b);if(c!=
null)return Tuple.Create(kvp.Value,c);}}return null;}public static Type FindType
(string a){q();Type b;if(o.TryGetValue(a,out b))return b;return r(a);}private
static void q(){if(o.Count==0){foreach(var assembly in p){if(assembly!=null){try
{foreach(var type in assembly.DefinedTypes)o[type.FullName]=type.AsType();}catch
(System.Exception e){L.q("Failed to load assembly types: "+e.Message);}}}}}
private static Type r(string a){var b=new[]{
"System.Net.Sockets, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
,
"System.Net.Sockets, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
,
"System.Net.Primitives, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
,
"System.Net.Primitives, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
,
"System.Net.Primitives, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
,};if(string.IsNullOrWhiteSpace(a))throw new InvalidOperationException(
"Type name shouldn't be empty or null");if(a.StartsWith("System.Net",
StringComparison.OrdinalIgnoreCase)){foreach(var netAssembly in b){try{var c=
Type.GetType(a+", "+netAssembly);if(c!=null)return c;}catch{}}}return null;}
public static FieldInfo FindBindablePropertyField(Type a,string b){var c=a.
GetTypeInfo();foreach(var declaredField in c.DeclaredFields)if(declaredField.
IsStatic&&declaredField.Name==b+"Property")return declaredField;if(c.BaseType==
null)return null;return FindBindablePropertyField(c.BaseType,b);}public static
ConstructorInfo GetConstructor(Type a,params Type[]b){var c=a.GetConstructors().
FirstOrDefault(d=>{var f=d.GetParameters();var g=f.Length;return g==b.Length&&f.
Zip(b,(h,i)=>h.ParameterType.IsAssignableFrom(i)).All(j=>j);});if(c==null)throw
new InvalidOperationException("Couldn't find constructor for "+a+
" with parameters: "+string.Join(", ",b.Select(k=>k.Name)));return c;}private
static Assembly[]s(){var a=typeof(string).GetTypeInfo();var b=a.Assembly;var c=b
.GetType("System.AppDomain");var d=c.GetRuntimeProperty("CurrentDomain");var f=d
.GetMethod;var g=f.Invoke(null,new object[]{});var h=g.GetType().
GetRuntimeMethod("GetAssemblies",new Type[]{});return h.Invoke(g,new object[]{})
as Assembly[];}}public class __internal{public static Listener _instance=
Listener.Instance;}public class Listener{private static Lazy<Listener>o=new Lazy
<Listener>(()=>new Listener());public static Listener Instance{get{return o.
Value;}}private readonly Q p=new Q();private readonly byte[]q=new byte[1024*100]
;private readonly Z r;private const int s=53032;private const int t=53050;
private string u;private a4 v;private string w="192.168.1.2";private Listener(){p.
MessageReceived+=D;r=new Z(new[]{w,"127.0.0.1","169.254.80.80","10.0.2.2",
"10.0.3.2"},y,x);}private void x(){L.p(
"Failed to find find a handshake server. Switching to UDP broadcast."+
Environment.NewLine+
"Make sure you have opened 53031 and 53032 ports for incoming TCP connections on your host PC."
);var a=new a6(t);a.BeginReceive(B,a);}private void y(string a){u=a;z(u);}
private void z(string a){var b=new a5();try{L.o("Listener connecting to "+a);b.s
(a,s).Wait(20);L.o("Listener connected to "+a);v=b.Client;A(v,0);}catch(
Exception e){L.o("Listener failed to connect to "+a);L.Exception(e);}}private
void A(a4 a,int b){try{if(b>=q.Length){b=0;}a.BeginReceive(q,b,q.Length-b,0,C,b)
;}catch(Exception e){L.WriteLine("BeginReceive failed");L.Exception(e);z(u);}}
private void B(IAsyncResult a){a6 b=null;try{b=(a6)a.AsyncState;var c=new a2(a1.
Any,t).Object;var d=b.EndReceive(a,ref c);p.Feed(d,d.Length);b.BeginReceive(B,b)
;}catch(Exception e){L.WriteLine("UdpEndReceive failed");L.Exception(e);if(b!=
null)((IDisposable)b.Object).Dispose();b=new a6(t);b.BeginReceive(B,b);}}private
void C(IAsyncResult a){try{var b=(int)a.AsyncState;var c=v.EndReceive(a);L.
WriteLine("Listener received a message ("+c+")");if(c==0){L.WriteLine(
"Connection to the server has ended");z(u);return;}p.Feed(q,c);A(v,0);}catch(
Exception e){L.WriteLine("EndReceive failed");L.Exception(e);z(u);}}private void
D(object a,S b){L.q("Listener message parsed");var c=b.Messages;T.Run(()=>{try{W
.ReceiveMessages(c);}catch(Exception ex){L.WriteLine("ReceiveMessages failed");L
.Exception(ex);}});}}internal class Q{public event EventHandler<S>
MessageReceived;private readonly List<byte>o=new List<byte>();private int p=0;
private int q;private string r;private int s;private byte[]t;private ushort u;
private byte v;private List<Message>w;private ushort x;private string y;private
ushort z;private DateTime A=DateTime.Now;public void Feed(byte[]a,int b){if((
DateTime.Now-A).TotalMilliseconds>1000)p=0;A=DateTime.Now;for(int c=0;c<b;c++)B(
a[c]);}private void B(byte a){switch(p){case 0:if(a==0xbe){w=new List<Message>()
;p=1;}break;case 1:if(a==0xef){p=2;}break;case 2:v=a;p=3;break;case 3:o.Add(a);
if(o.Count==4){q=BitConverter.ToInt32(o.ToArray(),0);o.Clear();if(q<=0||q>10000)
p=0;else p=4;}break;case 4:o.Add(a);if(o.Count==q){r=Encoding.Unicode.GetString(
o.ToArray(),0,o.Count);o.Clear();p=5;}break;case 5:o.Add(a);if(o.Count==4){s=
BitConverter.ToInt32(o.Take(4).ToArray(),0);o.Clear();p=6;if(s<=0||s>1024*1024){
p=0;}else{p=6;}}break;case 6:o.Add(a);if(o.Count==s){var b=o.ToArray();t=b;u=
Fletcher16(b);o.Clear();p=7;}break;case 7:o.Add(a);if(o.Count==2){var c=
BitConverter.ToUInt16(o.ToArray(),0);o.Clear();if(u==c){w.Add(new Message{
TargetId=r,Buffer=t});p=8;}else p=0;}break;case 8:o.Add(a);if(o.Count==2){x=
BitConverter.ToUInt16(o.ToArray(),0);o.Clear();if(x>0)p=9;else if(v>1){v--;p=3;}
else p=10;}break;case 9:o.Add(a);if(o.Count==x){w.Last().PropertyList=Encoding.
Unicode.GetString(o.ToArray(),0,o.Count);o.Clear();if(--v>0)p=3;else p=10;}break
;case 10:if(a==0xff){var d=MessageReceived;if(d!=null){var f=y==r&&z==u;z=u;y=r;
if(!f)d(this,new S{Messages=w});}}p=0;break;default:throw new
ArgumentOutOfRangeException();}}public ushort Fletcher16(byte[]a){ushort b=0;
ushort c=0;for(var d=0;d<a.Length;++d){b=(ushort)((b+a[d])%255);c=(ushort)((c+b)
%255);}return(ushort)((c<<8)|b);}}public class Message{public string TargetId{
get;set;}public byte[]Buffer{get;set;}public string PropertyList{get;set;}public
string OriginalTargetId{get;internal set;}internal W.XamlBuffer o{get;set;}}
internal class S:EventArgs{public List<Message>Messages{get;set;}}internal class
T{public static void Run(Action a){Device.BeginInvokeOnMainThread(a);}}internal
static class U{private static readonly HashSet<Type>o=new HashSet<Type>{typeof(
int),typeof(double),typeof(decimal),typeof(long),typeof(short),typeof(sbyte),
typeof(byte),typeof(ulong),typeof(ushort),typeof(uint),typeof(float)};private
static string[]p;public static string[]GetTypeNames(){if(p==null)p=o.Select(a=>a
.FullName).ToArray();return p;}public static bool TypeIsNumeric(Type a){return o
.Contains(Nullable.GetUnderlyingType(a)??a);}public static bool TypeIsNumeric(
string a){return o.Any(b=>b.FullName==a);}}internal static class V{public static
IEnumerable<MethodInfo>GetAllDeclaredMethods(this Type a){foreach(var method in
a.GetTypeInfo().DeclaredMethods)yield return method;var b=a.GetTypeInfo().
BaseType;if(b==null)yield break;foreach(var baseTypeMethod in
GetAllDeclaredMethods(b))yield return baseTypeMethod;}public static object
CallMethod<TArg0>(this object a,string b,TArg0 c){return o(a,b,new Type[]{typeof
(TArg0)},new object[]{c});}public static object CallMethod<TArg0,TArg1>(this
object a,string b,TArg0 c,TArg1 d){return o(a,b,new Type[]{typeof(TArg0),typeof(
TArg1)},new object[]{c,d});}public static object CallMethod<TArg0,TArg1,TArg2>(
this object a,string b,TArg0 c,TArg1 d,TArg2 f){return o(a,b,new Type[]{typeof(
TArg0),typeof(TArg1),typeof(TArg2)},new object[]{c,d,f});}public static object
CallMethod<TArg0,TArg1,TArg2,TArg3>(this object a,string b,TArg0 c,TArg1 d,TArg2
f,TArg3 g){return o(a,b,new Type[]{typeof(TArg0),typeof(TArg1),typeof(TArg2),
typeof(TArg3)},new object[]{c,d,f,g});}private static object o(object a,string b
,Type[]c,object[]d){var f=GetMethod(a.GetType(),b,true,c);if(f!=null)return f.
Invoke(a,d);throw new InvalidOperationException("Unable to call method "+b+
" on type "+a.GetType()+". Method not found");}public static MethodInfo
GetMethod(this Type a,string b,bool c,Type[]d=null){var f=a.GetAllMethods().
Where(g=>g.Name==b).FirstOrDefault(h=>(c?!h.IsStatic:h.IsStatic)&&p(h,d));if(f==
null)throw new Exception("Method `"+b+"` on type `"+a.Name+"` not found");return
f;}public static ConstructorInfo[]GetConstructors(this Type a){return a.
GetTypeInfo().DeclaredConstructors.ToArray();}public static ConstructorInfo
FindConstructor(this Type a,params Type[]b){return a.GetTypeInfo().
DeclaredConstructors.FirstOrDefault(c=>{var d=c.GetParameters();if(d.Length!=b.
Length)return false;return d.Zip(b,(f,g)=>f.ParameterType==g).All(h=>h);});}
public static bool IsAssignableFrom(this Type a,Type b){return a.GetTypeInfo().
IsAssignableFrom(b.GetTypeInfo());}public static object GetPropertyValue(this
object a,string b){var c=a.GetType().GetRuntimeProperties().FirstOrDefault(d=>d.
Name==b);if(c==null)return null;return c.GetValue(a);}public static object
GetFieldValue(this object a,string b){var c=a.GetType().GetRuntimeFields().
FirstOrDefault(d=>d.Name==b);if(c==null)return null;return c.GetValue(a);}public
static IEnumerable<FieldInfo>GetAllFields(this Type a){foreach(var fld in a.
GetRuntimeFields())yield return fld;var b=a.GetTypeInfo().BaseType;if(b!=null){
foreach(var baseTypeFld in GetAllFields(b)){yield return baseTypeFld;}}}private
static bool p(MethodInfo a,Type[]b){if(b==null)return true;var c=a.GetParameters
();if(c.Length!=b.Length)return false;for(var d=0;d<b.Length;d++)if(!c[d].
ParameterType.IsAssignableFrom(b[d]))return false;return true;}public static
IEnumerable<MethodInfo>GetAllMethods(this Type a){var b=a.GetTypeInfo();while(
true){foreach(var method in b.DeclaredMethods)yield return method;if(b.BaseType
!=null)b=b.BaseType.GetTypeInfo();else break;}}public static int
GetCustomControlAncestorCount(this Type a){var b=a.GetTypeInfo();if(b.BaseType==
null)return 0;var c=0;var d=b.BaseType;while(d!=null){if(IsCustomControl(d))c++;
d=d.GetTypeInfo().BaseType;}return c;}public static bool IsCustomControl(this
Type a){var b=a.GetTypeInfo();var c=b.DeclaredMethods;return c.FirstOrDefault(d
=>d.Name=="InitializeComponent")!=null;}}internal static class Runtime{public
static readonly BindableProperty RegisterProperty=BindableProperty.
CreateAttached("Register",typeof(string),typeof(Runtime),"",BindingMode.OneWay,
null,RegisterPropertyChanged);private static Listener _listenerInstance;private
static void RegisterPropertyChanged(BindableObject a,object b,object c){try{var
d=Device.OS==TargetPlatform.Android||Device.OS==TargetPlatform.iOS||Device.OS==
TargetPlatform.Windows||Device.OS==TargetPlatform.WinPhone;if(d&&
_listenerInstance==null)_listenerInstance=Listener.Instance;}catch(Exception e){
L.WriteLine(e.ToString());throw;}}public static void SetRegister(BindableObject
a,string b){a.SetValue(RegisterProperty,b);}public static string GetRegister(
BindableObject a){return(string)a.GetValue(RegisterProperty);}}internal static
class W{static readonly Dictionary<string,XamlBuffer>o=new Dictionary<string,
XamlBuffer>();static readonly Dictionary<string,string>p=new Dictionary<string,
string>();public static Element GetElementRoot(Element a){var b=a;while(b.Parent
!=null)b=b.Parent;return b;}public static XamlBuffer FindBuffer(string a){a=a.
ToLower();XamlBuffer b;if(o.TryGetValue(a,out b))return b;return null;}public
static void ReceiveMessages(List<Message>a){O.SetElementXamlFileProvider();L.q(
"Received "+a.Count+" messages");foreach(var message in a){L.q(
"Received message to '"+message.TargetId+"' with length "+message.Buffer.Length)
;message.OriginalTargetId=message.TargetId;message.TargetId=message.TargetId.
ToLower();var b=new List<string>();var c=_.TransformXaml(Encoding.Unicode.
GetString(message.Buffer,0,message.Buffer.Length),b);message.o=o[message.
TargetId]=new XamlBuffer(c,b);}if(a.Count==1){var d=new Y(a[0].OriginalTargetId,
a[0].o,a[0].PropertyList);O.u(a[0].OriginalTargetId,a[0].o.XamlString);q(d);
Application.Current.MainPage.Opacity=0;Application.Current.MainPage.FadeTo(1,350
);return;}L.WriteLine(
"Please update Visual Studio extension. This version doesn't support Visual Studio for Mac yet)"
);}private static void q(Y a){r(a,Application.Current,0);L.q(
"Updating controls ended");}private static X r(Y a,Element b,int c,ListView d=
null){if(a.TraversedObjects.Contains(b))return X.Normal();a.TraversedObjects.Add
(b);L.q(new string(' ',c*2)+b.GetType().Name);if(b is ListView&&d==null)d=(
ListView)b;if(b.GetType().FullName==a.TypeName){if(d!=null&&d!=b){O.u(b.GetType(
),a.XamlBuffer.XamlString);var f=u(d);if(f!=null){var g=new Y(a.TypeName,new
XamlBuffer("",new List<string>()),a.PropertyList);v(g,f);return X.JumpToAncestor
(f);}}v(a,b);return X.Normal();}ResourceDictionary h=null;var i=N.
GetMergedResourceDictionary(b,out h);if(i&&h.GetType().FullName==a.TypeName){O.u
(h.GetType(),a.XamlBuffer.XamlString);if(b is VisualElement)N.
CreateNewMergedResourceDictionary(((VisualElement)b).Resources);else if(b is
Application)N.CreateNewMergedResourceDictionary(((Application)b).Resources);var
j=u(d??b);if(j!=null){var g=new Y(a.TypeName,new XamlBuffer("",new List<string>(
)),a.PropertyList);v(g,j);return X.JumpToAncestor(j);}}if(b is VisualElement)s(a
,b.GetPropertyValue("NavigationProxy"),c,d);var k=b as IElementController;var l=
k!=null?(IEnumerable<Element>)k.LogicalChildren:new Element[0];var m=b is
ListView?t((ListView)b):l;foreach(var child in m){if(child==null)continue;var n=
r(a,child,c+1,d);if(n.Type==1&&child!=n.Object)return n;}return X.Normal();}
private static void s(Y a,object b,int c,ListView d=null){if(b==null){L.q(
"Couldn't find NavigationProxy");return;}var f=b.GetPropertyValue("ModalStack")
as IEnumerable;var g=b.GetPropertyValue("NavigationStack")as IEnumerable;if(f!=
null)foreach(var navPage in f.OfType<Page>())r(a,navPage,0);if(g!=null)foreach(
var navPage in g.OfType<Page>())r(a,navPage,0);}private static IEnumerable<
Element>t(ListView a){var b=a.GetPropertyValue("TemplatedItems");if(b!=null)
return((IEnumerable)b).OfType<Element>();return Enumerable.Empty<Element>();}
private static Element u(Element a){if(a.GetType().IsCustomControl())return a;if
(a.Parent!=null)return u(a.Parent);return null;}private static void v(Y a,
Element b){L.q("Updating control node "+a.TypeName);if(!string.
IsNullOrWhiteSpace(a.XamlBuffer.XamlString)){O.u(b.GetType(),a.XamlBuffer.
XamlString);ClearElement(b,a.TypeName,a.PropertyList);O.InitializeComponent(b,a.
XamlBuffer.XamlString);}else{O.InitializeComponent(b);}}public static void
ClearElement(object a,string b,string c){ClearChildren(a);var d=a as Element;if(
d!=null){var f=NameScope.GetNameScope(d)as NameScope;if(f!=null){}}if(c!=null)w(
a,b,c);}public static void ClearChildren(object a){try{if(a is ContentPage){((
ContentPage)a).Content=null;}else if(a is ContentView){((ContentView)a).Content=
null;}else if(a is ScrollView){((ScrollView)a).Content=null;}else if(a is
ContentPresenter){((ContentPresenter)a).Content=null;}else if(a is Frame){((
Frame)a).Content=null;}else if(a is StackLayout){((StackLayout)a).Children.Clear
();}else if(a is AbsoluteLayout){((AbsoluteLayout)a).Children.Clear();}else if(a
is RelativeLayout){((RelativeLayout)a).Children.Clear();}if(a is Page){var b=((
Page)a);if(b.ToolbarItems!=null)b.ToolbarItems.Clear();}if(a is Application){var
c=(Application)a;c.Resources.Clear();}}catch(Exception e){L.WriteLine(
"Unable to ClearChildren: "+e);}}public static string GetInitialPropertyList(
string a){string b;if(p.TryGetValue(a,out b))return b;return null;}private
static void w(object a,string b,string c){var d=a as Element;if(d==null)return;p
[b]=c;foreach(var property in c.Split(',')){if(string.IsNullOrWhiteSpace(
property))continue;L.q("ResetProperty "+property);var f=property;var g=f.
LastIndexOf('.');if(g!=-1){var h=f.Substring(0,g);var i=f.Substring(g+1);var j=P
.FindShortTypeWithProperty(h,i);if(j==null)continue;var k=j.Item2;var l=(
BindableProperty)k.GetValue(null);d.ClearValue(l);}else{var m=a.GetType();x(d,m,
property);}}}private static void x(Element a,Type b,string c){var d=
FindBindablePropertyField(b,c);if(d==null){L.WriteLine("Bindable property "+c+
" not found on type "+b.FullName);return;}var f=(BindableProperty)d.GetValue(
null);a.ClearValue(f);}public static FieldInfo FindBindablePropertyField(Type a,
string b){var c=a.GetTypeInfo();foreach(var declaredField in c.DeclaredFields)if
(declaredField.IsStatic&&declaredField.Name==b+"Property")return declaredField;
if(c.BaseType==null)return null;return FindBindablePropertyField(c.BaseType,b);}
private static HashSet<object>y(IList<object>a){var b=new HashSet<object>();
foreach(var element in a){var c=element as Element;if(c!=null){var d=z(c);var f=
d.LastOrDefault(g=>a.Any(h=>h!=element&&h==g));b.Add(f??element);}else{b.Add(
element);}}return b;}private static IList<Element>z(Element a){var b=new List<
Element>();var c=a.Parent;while(c!=null){b.Add(c);var d=c;c=d.Parent;}return b;}
public static IEnumerable<Element>GetLogicalDescendants(Element a){var b=(
IElementController)a;foreach(var child in b.LogicalChildren){yield return child;
foreach(var grandChild in GetLogicalDescendants(child))yield return grandChild;}
}private static XamlBuffer A(byte[]a,string b){var c=Encoding.Unicode.GetString(
a,0,a.Length);var d=new List<string>();try{c=_.TransformXaml(c,d,b);}catch(
Exception e){L.WriteLine("Failed to transform XAML");L.WriteLine(e.ToString());}
return new XamlBuffer(c,d);}private static void B(){try{Application.Current.
MainPage=(Page)Activator.CreateInstance(Application.Current.MainPage.GetType());
}catch(Exception e){L.WriteLine("Failed to reload MainPage");L.WriteLine(e.
ToString());}}public class XamlBuffer{public XamlBuffer(string a,List<string>b){
XamlString=a;XNames=b;}public string XamlString{get;set;}public IReadOnlyList<
string>XNames{get;set;}public string CellXamlString{get;internal set;}public
bool ResourceUpdated{get;internal set;}}struct X{public Element Object;public
int Type;public X(int a,Element b=null){Type=a;Object=b;}public static X Normal(
){return new X(0);}public static X JumpToAncestor(Element a){return new X(1,a);}
}internal class Y{public Y(string a,XamlBuffer b,string c){TypeName=a;XamlBuffer
=b;PropertyList=c;TraversedObjects=new HashSet<object>();}public string TypeName
{get;set;}public string PropertyList{get;set;}public XamlBuffer XamlBuffer{get;
set;}public HashSet<object>TraversedObjects{get;set;}}}internal class Z{private
const int o=53031;private readonly string[]p;private readonly Action q;private
readonly Action<string>r;private readonly byte[]s=new byte[1];private int t=-1;
private string u;private AutoResetEvent v=new AutoResetEvent(false);public Z(
string[]a,Action<string>b,Action c){p=a.Where(d=>!d.StartsWith("{")).ToArray();r
=b;q=c;w().ContinueWith(f=>{if(f.Exception!=null){L.o("ServerLocator error");L.o
(f.Exception.ToString());}});}private async Task w(){t++;if(t>p.Length-1){q();
return;}var a=p[t];try{var b=new a5();L.o("Connecting to the handshake server "+
a);var c=b.s(a,o);var d=await Task.WhenAny(c,Task.Delay(500)).ContinueWith(f=>{
if(f.Exception!=null){L.o("Handshake connection failed for "+a+" "+f.Exception.
Message);return false;}if(f.Result==null)return false;if(f.Result.Exception!=
null){L.o("Handshake connection failed for "+a+" "+f.Result.Exception.Message);
return false;}if(f.Result!=c){L.o("Handshake connection timed out");return false
;}return true;});if(d){var g=b.Client;var h=false;var i=new
CancellationTokenSource();g.BeginReceive(s,0,s.Length,0,j=>{i.Cancel();if(!h)y(j
,g);},a);var k=Task.Delay(1000,i.Token).ContinueWith(f=>{if(f.Exception==null&&f
.Status!=TaskStatus.Canceled){h=true;v.Set();}});await x();try{if(g.Object is
IDisposable){var l=(IDisposable)g.Object;l.Dispose();}}catch{L.o(
"Failed to properly dispose a socket for "+a);}}if(string.IsNullOrWhiteSpace(u))
{L.WriteLine("Handshake server unreachable "+a);await w();}else{r(u);}}catch(
Exception e){L.o("Handshake failed for "+a);L.Exception(e);await w();}}private
Task x(){return Task.Run(()=>v.WaitOne());}private void y(IAsyncResult a,a4 b){
try{var c=(string)a.AsyncState;var d=b.EndReceive(a);if(d==1&&s[0]==0xAA){u=c;v.
Set();}}catch(Exception e){L.WriteLine("Handshake EndReceive failed");L.
Exception(e);}}}class _{private const string o=
"http://schemas.microsoft.com/expression/blend/2008";public static string
TransformXaml(string a,List<string>b,string c=null){var d=new StringReader(a);
var f=new StringWriter();using(XmlReader g=XmlReader.Create(d)){using(XmlWriter
h=XmlWriter.Create(f)){var i=true;while(g.Read()){if(g.NodeType==XmlNodeType.
Element){q(g,h,i,b,c);i=false;}else{q(g,h,i,b,c);}}}}return f.ToString();}static
bool p(a0 a){return a.LocalName=="xmlns"||a.Prefix=="xmlns";}static void q(
XmlReader a,XmlWriter b,bool c,List<string>d,string f=null){switch(a.NodeType){
case XmlNodeType.Element:b.WriteStartElement(a.Prefix,a.LocalName,a.NamespaceURI
);var g=new a0[a.AttributeCount];int h=-1;for(int i=0;i<a.AttributeCount;i++){a.
MoveToAttribute(i);g[i].Prefix=a.Prefix;g[i].LocalName=a.LocalName;g[i].
NamespaceURI=a.NamespaceURI;g[i].Value=a.Value;if(a.LocalName=="xmlns")h=i;}if(h
!=-1)b.WriteAttributeString(g[h].Prefix,g[h].LocalName,null,g[h].Value);for(var
i=0;i<g.Length;i++){if(i==h)continue;if(g[i].NamespaceURI==o)continue;if(!p(g[i]
))continue;b.WriteAttributeString(g[i].Prefix,g[i].LocalName,null,g[i].Value);}
for(var i=0;i<g.Length;i++){if(i==h)continue;if(p(g[i]))continue;if(g[i].
NamespaceURI==o)continue;if(c&&g[i].LocalName=="AutomationId")continue;if(g[i].
LocalName=="Name"&&g[i].Prefix=="x")d.Add(g[i].Value);b.WriteAttributeString(g[i
].Prefix,g[i].LocalName,null,g[i].Value);}if(a.AttributeCount>0)a.MoveToElement(
);if(a.IsEmptyElement)b.WriteEndElement();break;case XmlNodeType.Text:b.
WriteString(a.Value);break;case XmlNodeType.Whitespace:case XmlNodeType.
SignificantWhitespace:break;case XmlNodeType.CDATA:b.WriteCData(a.Value);break;
case XmlNodeType.EntityReference:b.WriteEntityRef(a.Name);break;case XmlNodeType
.XmlDeclaration:case XmlNodeType.ProcessingInstruction:b.
WriteProcessingInstruction(a.Name,a.Value);break;case XmlNodeType.DocumentType:b
.WriteDocType(a.Name,a.GetAttribute("PUBLIC"),a.GetAttribute("SYSTEM"),a.Value);
break;case XmlNodeType.Comment:b.WriteComment(a.Value);break;case XmlNodeType.
EndElement:b.WriteFullEndElement();break;default:break;}}struct a0{public string
Prefix;public string LocalName;public string NamespaceURI;public string Value;}}
internal class a1{private static bool o;private static ConstructorInfo p;public
static object Any{get{if(!o)q();return p.Invoke(new object[]{0});}}private
static void q(){var a=P.FindType("System.Net.IPAddress");p=a.FindConstructor(
typeof(long));if(p==null)throw new Exception("IPAddress constructor not found");
o=true;}}internal class a2{public object Object{get;}public a2(object a,int b){
var c=P.FindType("System.Net.IPEndPoint");var d=P.FindType(
"System.Net.IPAddress");var f=c.FindConstructor(d,typeof(int));Object=f.Invoke(
new[]{a,b});}}internal class a4{public object Object{get;}private readonly
MethodInfo o;private readonly MethodInfo p;private readonly MethodInfo q;private
readonly MethodInfo r;private readonly PropertyInfo s;private readonly
PropertyInfo t;public int ReceiveTimeout{get{return(int)s.GetValue(Object);}set{
s.SetValue(Object,value);}}public bool Blocking{get{return(bool)t.GetValue(
Object);}set{t.SetValue(Object,value);}}private a4(object a){Object=a;var b=P.
FindType("System.Net.Sockets.Socket");var c=P.FindType(
"System.Net.Sockets.SocketFlags");q=b.GetMethod("Receive",true,new Type[]{typeof
(byte[])});o=b.GetMethod("BeginReceive",true,new Type[]{typeof(byte[]),typeof(
int),typeof(int),c,typeof(AsyncCallback),typeof(object)});p=b.GetMethod(
"EndReceive",true,new Type[]{typeof(IAsyncResult)});s=b.GetRuntimeProperty(
"ReceiveTimeout");t=b.GetRuntimeProperty("Blocking");}public static a4
FromObject(object a){return new a4(a);}public int Receive(byte[]a){return(int)q.
Invoke(Object,new[]{a});}public IAsyncResult BeginReceive(byte[]a,int b,int c,
int d,AsyncCallback f,object g){return(IAsyncResult)o.Invoke(Object,new[]{a,b,c,
(int)d,f,g});}public int EndReceive(IAsyncResult a){return(int)p.Invoke(Object,
new object[]{a});}}internal class a5{private MethodInfo o;private PropertyInfo p
;private a4 q;private object r{get;}public a4 Client{get{if(r==null)throw new
InvalidOperationException("Object is not initialized yet");return q??(q=a4.
FromObject(p.GetValue(r)));}}public a5(){var a=P.FindType(
"System.Net.Sockets.TcpClient");if(a==null){L.p(
"Couldn't find TcpClient type. Please make sure linking is disabled for iOS/Android project."
);}var b=P.GetConstructor(a);o=a.GetMethod("ConnectAsync",true,new Type[]{typeof
(string),typeof(int)});p=a.GetRuntimeProperty("Client");r=b.Invoke(new object[0]
);}internal Task s(string a,int b){return(Task)o.Invoke(r,new object[]{a,b});}}
internal class a6{private MethodInfo o;private MethodInfo p;public object Object
{get;private set;}public a6(int a){var b=P.FindType(
"System.Net.Sockets.UdpClient");var c=P.GetConstructor(b,typeof(int));var d=P.
FindType("System.Net.IPEndPoint");o=b.GetMethod("BeginReceive",true,new Type[]{
typeof(AsyncCallback),typeof(object)});p=b.GetMethod("EndReceive",true,new Type[
]{typeof(IAsyncResult),d.MakeByRefType()});Object=c.Invoke(new object[]{a});}
public void BeginReceive(AsyncCallback a,object b){o.Invoke(Object,new object[]{
a,b});}public byte[]EndReceive(IAsyncResult a,ref object b){return(byte[])p.
Invoke(Object,new object[]{a,b});}} } 