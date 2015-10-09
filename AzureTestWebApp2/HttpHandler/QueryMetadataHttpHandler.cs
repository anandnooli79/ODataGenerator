using AzureTestWebApp2.RouteHandler;
using AzureTestWebDataLayer;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.OData.Builder;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;

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
            //if (!HttpContext.Current.Request.IsAuthenticated)
            //{
            //    HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = HttpContext.Current.Request.FilePath }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            //    return;
            //}
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

        public static Microsoft.Data.Edm.IEdmModel BuildODataModel()
        {
            ODataModelBuilder modelBuilder = new ODataModelBuilder();


            var myType = TypeBuilderNamespace.MyTypeBuilder.CompileResultType();
            var entityTypeConfiguration = new EntityTypeConfiguration(modelBuilder, typeof(MsrRecurringQuery));

            //  var item = Expression.Parameter(typeof(MsrRecurringQuery));

            //property of my item, this is "item.Name"
            //  var prop = Expression.Property(item, "RecurringQueryID");

            //  var lambda = Expression.Lambda<Func<myType, int>>(prop, item);

            // var lambda = CreateLambda(myType, prop, item);// Expression.Lambda<Func<myType, int>>(prop);


            //var customers = CreateEntitySet(new MsrRecurringQuery(), modelBuilder, "MsrRecurringQueries");

            //var propertyInfo = typeof(MsrRecurringQuery).GetProperties()[0];

            //entityTypeConfiguration.HasKey(propertyInfo);
            //propertyInfo = typeof(MsrRecurringQuery).GetProperties()[1];
            //entityTypeConfiguration.AddProperty(propertyInfo);
            
            //var customers1 = modelBuilder.AddEntitySet("MsrRecurringQueries1", entityTypeConfiguration);

            Microsoft.Data.Edm.Library.EdmModel mainModel = new Microsoft.Data.Edm.Library.EdmModel();
            var mainContainer = new EdmEntityContainer("mainNS", "MainContainer");
           
            var msrRecurringQueryResultType = new EdmEntityType("mainNS", "MsrRecurringQuery", null);
            IEdmPrimitiveType edmPrimitiveType1 = new MSRAEdmPrimitiveType("Int32", "Edm", EdmPrimitiveTypeKind.Int32, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
            IEdmPrimitiveType edmPrimitiveType2 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
            IEdmPrimitiveType edmPrimitiveType3 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
            IEdmPrimitiveType edmPrimitiveType4 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
            IEdmPrimitiveType edmPrimitiveType5 = new MSRAEdmPrimitiveType("String", "Edm", EdmPrimitiveTypeKind.String, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
            IEdmPrimitiveType edmPrimitiveType6 = new MSRAEdmPrimitiveType("Decimal", "Edm", EdmPrimitiveTypeKind.Decimal, EdmSchemaElementKind.TypeDefinition, EdmTypeKind.Primitive);
            msrRecurringQueryResultType.AddKeys(new EdmStructuralProperty(msrRecurringQueryResultType, "RowId", new EdmPrimitiveTypeReference(edmPrimitiveType1, false)));
            msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "RowId", new EdmPrimitiveTypeReference(edmPrimitiveType1, false)));

            msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Pricing_Level", new EdmPrimitiveTypeReference(edmPrimitiveType2, false)));
            msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Business_Summary", new EdmPrimitiveTypeReference(edmPrimitiveType3, false)));
            msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Future_Flag", new EdmPrimitiveTypeReference(edmPrimitiveType4, false)));
            msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "Fiscal_Month", new EdmPrimitiveTypeReference(edmPrimitiveType5, false)));
            msrRecurringQueryResultType.AddProperty(new EdmStructuralProperty(msrRecurringQueryResultType, "MS_Sales_Amount_Const", new EdmPrimitiveTypeReference(edmPrimitiveType6, false)));
            mainModel.AddElement(msrRecurringQueryResultType);
            
             var customerSet = new EdmEntitySet(mainContainer, "MsrRecurringQueries", msrRecurringQueryResultType);
            mainContainer.AddElement(customerSet);
            mainModel.AddElement(mainContainer);
             
            return mainModel;
            //return modelBuilder.GetEdmModel();
        }

        public static EntitySetConfiguration<MsrRecurringQuery> CreateEntitySet(
          MsrRecurringQuery value, ODataModelBuilder modelBuilder, string entitySetName) //where T : class
        {
            var customers = modelBuilder.EntitySet<MsrRecurringQuery>(entitySetName);
            //customers.EntityType.HasKey(c => typeof(MsrRecurringQuery).GetProperties()[1]);
            customers.EntityType.HasKey(c =>c.RecurringQueryID);
            return customers;
        }

        public static Expression<Func<T, int>> CreateLambda<T>(
         T value, MemberExpression prop,ParameterExpression item) where T :class
        {
            return Expression.Lambda<Func<T, int>>(prop, item );
        }
    }

    public class MSRAEdmPrimitiveType : IEdmPrimitiveType
    {
        string _name = string.Empty;
        string _namesSpace = string.Empty;
        EdmPrimitiveTypeKind _primitiveKind = EdmPrimitiveTypeKind.String;
        EdmTypeKind _typeKind = EdmTypeKind.Primitive;
        EdmSchemaElementKind _schemaElementKind = EdmSchemaElementKind.None;
        public MSRAEdmPrimitiveType(string name,string namesSpace, EdmPrimitiveTypeKind primitiveKind, EdmSchemaElementKind schemaElementKind, EdmTypeKind typeKind )
        {
            _name = name;
            _namesSpace = namesSpace;
            _primitiveKind = primitiveKind;
            _typeKind = typeKind;
            _schemaElementKind = schemaElementKind;
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Namespace
        {
            get
            {
                return _namesSpace;
            }
        }

        public EdmPrimitiveTypeKind PrimitiveKind
        {
            get
            {
                return _primitiveKind;
            }
        }

        public EdmSchemaElementKind SchemaElementKind
        {
            get
            {
                return _schemaElementKind;
            }
        }

        public EdmTypeKind TypeKind
        {
            get
            {
                return _typeKind;
            }
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

      

      
    }
}
