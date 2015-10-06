using AzureTestWebApp2.RouteHandler;
using AzureTestWebDataLayer;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Http.OData.Builder;
using System.Reflection;
using System.Reflection.Emit;


namespace AzureTestWebApp2.HttpHandler
{
    public class QueryMetadataHttpHandler:IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

         public HttpContextBase ContextBase { get; private set; }

         public QueryMetadataHttpHandler(HttpContextBase context)
        {
            this.ContextBase = context;
        }
        public void ProcessRequest(HttpContext context)
        {
            MSRAHttpResponseMessage message = new MSRAHttpResponseMessage(this.ContextBase.Response);
            message.StatusCode = 200;

            message.SetHeader(ODataConstants.ContentTypeHeader, "application/xml");
            // create the writer, indent for readability
            ODataMessageWriterSettings messageWriterSettings = new ODataMessageWriterSettings()
            {
                Indent = true,
                CheckCharacters = false,
                BaseUri = context.Request.Url,
            };
            //AzureTestDBEntities db = new AzureTestDBEntities();
            //var queries = db.MsrRecurringQueries.ToList().Take(5);

            var eModel = BuildODataModel();
           
           //var schemaElements = new List<IEdmSchemaElement>();
           //var edmEntitType = new EntityType();;
           // edmEntitType.Name = "Query1";
           // edmEntitType.AddKeyMember(new EdmMember(){})
           // schemaElements.Add(new EdmEntityType("QueryResutls","Query1"){})
           // schemaElements.Add(EdmElement )
            using (ODataMessageWriter messageWriter = new ODataMessageWriter(message, messageWriterSettings, eModel))
            {
                messageWriter.WriteMetadataDocument();
            }
        }

        private static Microsoft.Data.Edm.IEdmModel BuildODataModel()
          {
             ODataModelBuilder modelBuilder = new ODataModelBuilder();

             var entityTypeConfiguration = new EntityTypeConfiguration(modelBuilder, typeof(MsrRecurringQuery));
             Type types = TypeBuilderNamespace.MyTypeBuilder.CompileResultType();
            
             //EntitySetConfiguration customers = modelBuilder.AddEntitySet("MsrRecurringQueries",entityTypeConfiguration);
            //modelBuilder.EntitySet<MsrRecurringQuery>("MsrRecurringQueries");
             //EntitySetConfiguration customers = modelBuilder.AddEntitySet("MsrRecurringQueries", new EntityTypeConfiguration() { J});
             var customers = modelBuilder.EntitySet<MsrRecurringQuery>("MsrRecurringQueries");
             string s = string.Empty;
             var myType = TypeBuilderNamespace.MyTypeBuilder.CompileResultType();

             TypeBuilderNamespace.MyTypeBuilder.CreateEntitySetFromSingle(myType);
             //customers.EntityType.HasKey(typeof(MsrRecurringQuery).GetProperty("RecurringQueryID"));
             customers.EntityType.HasKey(k => k.RecurringQueryID);
             return modelBuilder.GetEdmModel();
          }
    }
}




namespace TypeBuilderNamespace
{
    public static class MyTypeBuilder
    {
        public static void CreateNewObject()
        {
            var myType = CompileResultType();
            var myObject = Activator.CreateInstance(myType);
        }
        public static Type CompileResultType()
        {
            TypeBuilder tb = GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var yourListOfFields = CreateListFromSingle(
           new
           {
               FieldName = "RecurringQueryID",
               FieldType = Type.GetType("System.Int32"),
               IsKey = true
           }
                        );

            yourListOfFields.Add(new
            {
                FieldName = "RecurringQueryName",
                FieldType = Type.GetType("System.String"),
                IsKey = false
            }
                        );

            yourListOfFields.Add(new
            {
                FieldName = "PerspectiveId",
                FieldType = Type.GetType("System.Int16"),
                IsKey = false
            }
                       );

            yourListOfFields.Add(new
            {
                FieldName = "SelectedAttributes",
                FieldType = Type.GetType("System.String"),
                IsKey = false
            }
                       );

            yourListOfFields.Add(new
            {
                FieldName = "AttributeFilters",
                FieldType = Type.GetType("System.String"),
                IsKey = false
            }
                     );
            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            foreach (var field in yourListOfFields)
                CreateProperty(tb, field.FieldName, field.FieldType, field.IsKey);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder()
        {
            var typeSignature = "MyDynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType, bool isKey)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
            if (isKey)
            {
                var customAttributeBuilder = new CustomAttributeBuilder(typeof(System.ComponentModel.DataAnnotations.KeyAttribute).GetConstructor(new Type[] { }), new object[] { });
                propertyBuilder.SetCustomAttribute(customAttributeBuilder);
            }
        }

        static List<T> CreateListFromSingle<T>(T value) {
          var list = new List<T>();
          list.Add(value);
          return list;
        }

        public static void CreateEntitySetFromSingle<T>(T value) where T: class 
        {
            ODataModelBuilder modelBuilder = new ODataModelBuilder();

            modelBuilder.EntitySet<T>("NewName");
        }
    }
}
