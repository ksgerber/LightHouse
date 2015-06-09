﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightHouse.Core.Collections;
using LightHouse.Core.Generating.Native.Services.GenerateCode;
using LightHouse.Core.Generating.Native.Services.Templates;
using LightHouse.Execution;
using LightHouse.Localization;
using LightHouse.Model;

namespace LightHouse.Core.Generating.Native.Services
{
    /// <summary>
    /// Service repsonsible for generating code.
    /// </summary>
    public class GenerateCodeService : ServiceObject
    {
        public GenerateCodeService() : base() { }
        public GenerateCodeService(Boolean isProxy) : base(isProxy) { }
        public GenerateCodeService(DataObject dataObject) : base(dataObject) { }
        public GenerateCodeService(ContractObject contractObject) : base(contractObject) { }

        /// <summary>
        /// Element to be generated by the service.
        /// </summary>
        public virtual Element Element
        {
            get { return GetContractProperty<Element>(String.Format("{0}.{1}", typeof(GenerateCodeService).FullName, "Element")); }
            set { SetContractProperty(String.Format("{0}.{1}", typeof(GenerateCodeService).FullName, "Element"), value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if the service can be invoked; otherwise false.</returns>
        public override bool CanInvoke()
        {
            if (this.Element != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Invokes the generation service.
        /// </summary>
        /// <returns>Results from the invocation of the service.</returns>
        public ServiceResults Invoking()
        {
            return new GenerateCodeServiceResults()
            {
                GeneratedCode = GenerateCodeFromElement(this.Element)
            };
        }

        /// <summary>
        /// Gets the type from the provided value.
        /// </summary>
        /// <param name="value">Value to be analyzed for the type.</param>
        /// <returns>Returns the type of the value.</returns>
        internal static Type GetTypeFromValue(Value value)
        {
            Type type = value.GetType();

            switch (type.Name)
            {
                case "DateTimeValue":
                    return typeof(DateTime);
                case "CategoryObjectListValue":
                    return typeof(IContractList<CategoryObject>);
                case "CategoryObjectValue":
                    return typeof(CategoryObject);
                case "EntityObjectListValue":
                    return typeof(IContractList<EntityObject>);
                case "EntityObjectValue":
                    return typeof(EntityObject);
                case "GuidValue":
                    return typeof(Guid);
                case "LocalStringValue":
                    return typeof(LocalString);
                case "LocalTextValue":
                    return typeof(LocalText);
                case "BooleanValue":
                    return typeof(Boolean);
                case "DecimalValue":
                    return typeof(Decimal);
                case "IntegerValue":
                    return typeof(Int32);
                case "StringValue":
                    return typeof(String);
                case "ValueObjectListValue":
                    return typeof(IContractList<ValueObject>);
                case "ValueObjectValue":
                    return typeof(ValueObject);
                default:
                    return null;
            }

        }

        /// <summary>
        /// Generats code for the provided element.
        /// </summary>
        /// <param name="element">Element to be used for generating code.</param>
        /// <returns>Generated code as string.</returns>
        private string GenerateCodeFromElement(Element element)
        {
            if (element.IsContract)
            {
                ContractObjectTemplate template = new ContractObjectTemplate();
                template.Session = new Dictionary<string, object>();
                template.Session.Add("Element", element);
                template.Initialize();
                return template.TransformText();
            }
            else
            {
                DataObjectTemplate template = new DataObjectTemplate();
                template.Session = new Dictionary<string, object>();
                template.Session.Add("Element", element);
                template.Initialize();
                return template.TransformText();
            }
        }
    }
}