﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using BE;
//using DS;
//namespace DAL
//{
    
//    public class DAL_XML_Imp : Idal
//    {
//        static uint nextContractID;
//        static uint nextSpecID;

//        static DAL_XML_Imp()
//        {
//            setNextID(XML_Source.contractRoot, out nextContractID, 100000);
//           // setNextID(XML_Source.specializationRoot, out nextSpecID, 100000);
//        }

//        static void setNextID(XElement XRoot, out uint nextParam, uint defaultNext)
//        {
//            if (XRoot.HasElements == false) // no children nodes
//            {
//                nextParam = defaultNext;
//            }
//            else
//                nextParam = (from node in XRoot.Elements()
//                             where node.Attributes("ID").Any()
//                             select (uint)node.Attribute("ID")).Max() + 1;
//        }

//        // check if element already exists in XML file
//        XElement ElementIfExists(XElement root, XElement element)
//            => (from spec in root.Elements()
//                where spec.Attribute("ID").Value == element.Attribute("ID").Value
//                select spec).FirstOrDefault();

//        // overloaded method finds element based on ID
//        XElement ElementIfExists(XElement root, uint ID)
//            => (from spec in root.Elements()
//                where uint.Parse(spec.Attribute("ID").Value) == ID
//                select spec).FirstOrDefault();

//        bool removeElementFromXML(XElement XRoot, XElement element)
//        {
//            XElement foundElement = ElementIfExists(XRoot, element);
//            if (foundElement != null) // element found
//            {
//                foundElement.Remove();
//                //XML_Source.SaveXML<Specialization>();
//                return true;
//            }
//            else
//                throw new Exception("element " + element.Attribute("ID") + " not found in XML");
//        }

//        bool removeElementFromXML(XElement XRoot, uint ID)
//        {
//            XElement foundElement = ElementIfExists(XRoot, ID);
//            if (foundElement != null) // element found
//            {
//                foundElement.Remove();
//                //XML_Source.SaveXML<Specialization>();
//                return true;
//            }
//            else
//                throw new Exception("element " + ID + " not found in XML");
//        }

   

//        public uint getNextSpecID() => nextSpecID;

//        XElement createMotherXElement(Mother m)
//            =>    new XElement("mother"              ,new XAttribute("ID", m.ID),
//                  new XElement("firstName"           ,m.Firstname),
//                  new XElement("lastName"            ,m.Lastname),
//                  new XElement("phoneNumber"         ,m.Phonenumber),
//                  new XElement("Address"             ,m.Adress),
//                  new XElement("surrounding_adress"  , m.surrounding_adress),
//                  new XElement("Phonenumber"         , m.Phonenumber),
//                  new XElement("Paymentmethode"      , m.Paymentmethode),
//                  new XElement("nanny_required"      , m.nanny_required),
//                  new XElement("daily_Nanny_required", m.daily_Nanny_required),
//                  new XElement("Comment"             , m.Comment)
//                );

//        public bool addMother(Mother m)
//        {
//            if (ElementIfExists(XML_Source.motherRoot, (uint)m.ID) != null)
//            {
//                throw new Exception(m.ID + " already exists in file");
//            }

//            XML_Source.motherRoot.Add(createMotherXElement(m));
//            XML_Source.SaveXML<Mother>();
//            return true;
//        }
//        public bool deleteMother(Mother mother)
//            => removeElementFromXML(XML_Source.motherRoot,(uint)mother.ID);

//        public bool updateMother(Mother mother)
//        {
//            XElement foundElement = ElementIfExists(XML_Source.motherRoot, (uint)mother.ID);
//            if (foundElement == null)
//                throw new Exception(mother.ID + " does not exist in XML");

//            return
//                removeElementFromXML(XML_Source.motherRoot, (uint)foundElement)
//                && addMother(mother);
//        }

//        XElement createContractXElement(Contract c)
//            => new XElement("contract", new XAttribute("ID", c.Contract_ID),
//                 new XElement("ChildID", c.Child_ID),
//                 new XElement("MotherID", c.Mother_ID),
//                 new XElement("Contract_number", c.Contract_number),
//                 new XElement("contract_signed", c.contract_signed),
//                 new XElement("discount", c.discount),
//                 new XElement("distance", c.distance),
//                 new XElement("enddate", c.enddate),
//                  new XElement("Hourly_payment", c.Hourly_payment), 
//                  new XElement("introduce_meeting", c.introduce_meeting),
//                  new XElement("Monthly_payment", c.Monthly_payment),
//                  new XElement("Nanny_ID", c.Nanny_ID),
//                  new XElement("Paymentmethode", c.Paymentmethode),
//                  new XElement("salary", c.salary),
//                  new XElement("startdate", c.startdate)
//                 );

//        public bool addContract(Contract contract, bool autoAssignID = true)
//        {
//            if (ElementIfExists(XML_Source.contractRoot, (uint)contract.Contract_ID) != null)
//            {
//                throw new Exception(contract.Contract_ID + " already exists in file");
//            }

//            if (autoAssignID)
//                contract.Contract_ID =(int)nextContractID++;

//            XML_Source.contractRoot.Add(createContractXElement(contract));
//            XML_Source.SaveXML<Contract>();

//            return true;
//        }

//        public bool deleteContract(Contract contract)
//            => removeElementFromXML(XML_Source.contractRoot,(uint)contract.Contract_ID);


//        public bool updateContract(Contract contract)
//        {
//            XElement foundElement = ElementIfExists(XML_Source.contractRoot,(uint)contract.Contract_ID);
//            if (foundElement == null)
//                throw new Exception(contract.Contract_ID + " does not exist in XML");

//            return
//                removeElementFromXML(XML_Source.contractRoot, (uint)foundElement)
//                && addContract(contract, false); // don't assign new contract ID
//        }

//        public uint getNextContractID()
//        {
//            return nextContractID;
//        }

//        XElement createChildXElement(Child c)
//        {
//            XElement child = new XElement("child", new XAttribute("ID",c.ID),
//                                    new XElement("Birthdate",c.Birthdate),
//                                    new XElement("Mother_ID",c.Mother_ID),
//                                    new XElement("name", c.name),
//                                    new XElement("SpecialNeeds", c.SpecialNeeds)
//                                   );
//            // if private person, add first name and last name
//            return child;
//        }

//        public bool addChild(Child c)
//        {
//            if (ElementIfExists(XML_Source.childRoot, (uint)c.ID) != null)
//            {
//                throw new Exception(c.ID + " already exists in file");
//            }

//            XML_Source.childRoot.Add(createChildXElement(c));
//            XML_Source.SaveXML<Child>();
//            return true;
//        }

//        public bool deleteChild(Child child)
//            => removeElementFromXML(XML_Source.childRoot, (uint)child.ID);

//        public bool updateChild(Child child)
//        {
//            XElement foundElement = ElementIfExists(XML_Source.childRoot, (uint)child.ID);
//            if (foundElement == null)
//                throw new Exception(child.ID + " does not exist in XML");

//            return
//                removeElementFromXML(XML_Source.childRoot, (uint)foundElement)
//                && addChild(child);
//        }

     

//        public List<Mother> getMotherList()
//        {
//            try
//            {
//                return (from m in XML_Source.motherRoot.Elements()
//                        let currentMother = new Mother()
//                        {
//                            Firstname = (string)m.Element("Mother").Element("Firstname"),
//                            Lastname = (string)m.Element("Mother").Element("Lastname")
//                        }
//                        select new Mother()
//                        {

//                        ID                 = (int)m.Attribute("ID"),
//                        Firstname          = (string)m.Element("Firstname"),
//                        Lastname            = (string)m.Element("Lastname"),
//                        Phonenumber         = (string)m.Element("Phonenumber"),
//                        Adress           =   (Address)m.Element("Address"),
//                        surrounding_adress  =(strint) m.Element("surrounding_adress"),
//                        Paymentmethode      = (bl)m.Element("Paymentmethode"),
//                        nanny_required      = m.Element("nanny_required"),
//                        daily_Nanny_required= m.Element("daily_Nanny_required"),
//                        Comment             = m.Element("Comment")
//                        }).ToList();
//            }
//            catch
//            {
//                throw new Exception("getmotherList() exception");
//            }
//        }

//        public List<Child> getChildList()
//        {
//            try
//            {
//                return (from c in XML_Source.childRoot.Elements()
//                        select new Child()
//                        {
//                            ID = uint.Parse(c.Attribute("ID").Value),
//                            firstName = (string)e.Element("firstName"), // check if exists perhaps
//                            lastName = (string)e.Element("lastName"),
//                            phoneNumber = (string)e.Element("phoneNumber"),
//                            specializationID = uint.Parse(c.Element("specializationID").Value),
//                            establishmentDate = DateTime.Parse(c.Element("establishmentDate").Value),
//                            address = (CivicAddress)e.Element("CivicAddress") // calls explicit converter of Xlement to CivicAddress
//                        }
//                        ).ToList();
//            }
//            catch
//            {
//                throw new Exception("getChildList() exception");
//            }
//        }

//        public List<Contract> getContractList()
//        {
//            try
//            {
//                return (from cont in XML_Source.contractRoot.Elements()
//                        where cont.Name == "contract"
//                        select new Contract()
//                        {
//                            Contract_ID = (int)cont.Attribute("ID"),
//                            Child_ID = (int)cont.Element("ChildID"),
//                            Mother_ID = (uint)cont.Element("mother_ID"),
//                            Contract_number= cont.Element("Contract_number").
//                            isInterviewed = (bool)cont.Element("isInterviewed"),
//                            contractFinalized = (bool)cont.Element("contractFinalized"),
//                            grossWagePerHour = (double)cont.Element("grossWagePerHour"),
//                            netWagePerHour = (double)cont.Element("netWagePerHour"),
//                            contractEstablishedDate = (DateTime)cont.Element("contractEstablishedDate"),
//                            contractTerminatedDate = (DateTime)cont.Element("contractTerminatedDate"),
//                            maxWorkHours = (uint)cont.Element("maxWorkHours")
                           
//                        }).ToList();
//            }
//            catch { throw new Exception("getContractList() exception"); }
//        }

//        public List<Nanny> getNannyList()
//        {
//            var returnList = XML_Source.Nannys?.ToList();

//            if (returnList == null)
//                return new List<Nanny>();
//            else
//                return returnList;
//        }

//        public DoWorkDelegate getXMLNannyBackground_DoWork()
//            => XML_Source.downloadNannyXml;
//    }
//}
