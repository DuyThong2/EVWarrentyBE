# ⚙️ EVWarranty System



## 🧭 Overview

**EVWarranty System** is a distributed **Microservices-based platform** for managing vehicle information, parts catalog, and warranty workflows.  

The system is designed to ensure scalability, flexibility, and fault isolation using **YARP API Gateway** and **RabbitMQ (MassTransit)** for asynchronous communication between services.



---



## 🏗️ System Architecture



### 🧩 Microservices Style

- Each domain (User, Vehicle, Part Catalog, Warranty) is implemented as an independent microservice.

- **API Gateway (YARP)** handles centralized routing, authentication, and request aggregation.

- **Message Broker (RabbitMQ + MassTransit)** enables asynchronous communication for background jobs and inter-service messaging.

- Designed for **fault tolerance**, **independent scalability**, and **future extensibility**.



### 🧠 Core Components

| Component | Description |

|------------|-------------|

| **YARP API Gateway** | Routes client requests to appropriate microservices. |

| **User Service** | Manages authentication, roles, and user profiles. |

| **Vehicle Service** | Handles vehicle registration, ownership, and service history. |

| **Part Catalog Service** | Manages parts, packages, categories, and warranty policies. |

| **Warranty Service** | Handles warranty claims, validations, and service center actions. |

| **RabbitMQ + MassTransit** | Provides message-based communication between services. |

| **Database** | Each service has its own schema (isolated per domain). |



---



## 🔄 Business Flows



### 🅰️ a) Warranty Flow

1. Customer submits a **warranty claim** through the system.  

2. The **Warranty Service** validates the claim (checks warranty policy, vehicle data, and parts).  

3. **Vehicle Service** confirms ownership and mileage.  

4. **Part Catalog Service** verifies the part/package warranty policy.  

5. Once approved, a **Service Request** is generated and processed asynchronously via RabbitMQ.  

6. Customer receives claim status updates through the **API Gateway**.



---



### 🅱️ b) Service Flow

1. Service center receives a **maintenance/service request**.  

2. **Vehicle Service** provides vehicle details and history.  

3. **Part Catalog Service** suggests compatible parts/packages.  

4. The **Warranty Service** validates if parts are under warranty.  

5. Service report is generated and stored, with messages sent to RabbitMQ for notification and analytics updates.



---



### 🆎 c) Recall / Service Campaign Flow

1. Manufacturer issues a **recall campaign** for specific models or parts.  

2. **Part Catalog Service** identifies affected parts and packages.  

3. **Vehicle Service** locates vehicles that match recall conditions.  

4. Notifications are sent to **owners via User Service** and **service centers**.  

5. **Warranty Service** logs recall activities and tracks completion status asynchronously.



---



## 🗂️ Service Databases



Each microservice has an independent database.  

Example links (replace with your actual GitHub paths):



| Service | Database Script |

|----------|------------------|

| **User Service** | [UserDb.sql](https://github.com/your-org/evwarranty-system/blob/main/db/UserDb.sql) |

| **Vehicle Service** | [VehicleDb.sql](https://github.com/your-org/evwarranty-system/blob/main/db/VehicleDb.sql) |

| **Part Catalog Service** | [PartCatalogDb.sql](https://github.com/your-org/evwarranty-system/blob/main/db/PartCatalogDb.sql) |

| **Warranty Service** | [WarrantyDb.sql](https://github.com/your-org/evwarranty-system/blob/main/db/WarrantyDb.sql) |



---