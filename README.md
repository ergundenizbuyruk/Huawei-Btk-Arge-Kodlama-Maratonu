![image](https://github.com/ergundenizbuyruk/Huawei-Btk-Arge-Kodlama-Maratonu/assets/83646458/81ecccd2-a76e-4f87-8f11-38124f16dcea)# huawei-btk-arge-kodlama-maratonu

[DEMO] Mind Benders: Personalized Product Analysis with Artificial Intelligence
 Personalized Product Analysis with Artificial Intelligence
Our application is designed to help users make informed and healthy choices when purchasing products. By uploading a photo of the product's ingredients, the user can get a detailed analysis of how suitable and beneficial the product is for them. The application scans the content of the product with artificial intelligence systems and identifies substances that may cause allergies or adverse effects that the user has previously identified. It provides the user with a summary of the product's content and the presence or absence of the substances they have identified.
The application also sends warnings and suggestions via e-mail at certain intervals according to the user's preferences and needs. In addition, our application aims to help visually impaired people and people travelling abroad to recognise products and learn more easily about their contents. Our app aims to promote a healthy lifestyle and empower users with the information they need to buy products that are good for them.

Example

![image](https://github.com/ergundenizbuyruk/Huawei-Btk-Arge-Kodlama-Maratonu/assets/83646458/c49999e6-0a43-439f-acba-eb8cabb855d5)

                                     
                                            
The product is a snack made of corn meal, edible vegetable oil, spices, and condiments. It contains approximately 469.5 kcal of energy, 7.0 g of protein, 71.0 g of total carbohydrates, and 3.59 g of sugars per 100 g serving. The product also contains milk solids, maltodextrin, dextrose, and acidity regulators.
Unwanted Substance Analysis:
The product contains the following unwanted substances:
Lactose: The product contains milk solids, which means it contains lactose. This may be a problem for individuals who are lactose intolerant or have other dairy allergies.
Gluten: The product does not contain gluten, which is a relief for individuals with gluten-related allergies or intolerances. 
Hazelnut: The product does not contain hazelnut, which is a common allergen.






Technical Architecture of the Project:
 


Functıons
•	We have two applications in our system, one AI and one MVC. In the MVC application, users can register and login to the system.
•	 They can save the product picture they have taken to the system.
•	Then this application sends the photo with the person's information to the AI application.
•	 The artificial intelligence application first converts the contents of the photo into text.
•	 Then, with this text and person information, it analyzes how compatible this product is with the person and makes a detailed analysis about the product. 
AI field Steps
o	Making image to text with EasyOcr library with the image given by the use 
	Many alternatives such as Huawei OCR have been tried before this stage
o	Transforming the resulting string value into valuable information for the user by giving it to Meta and Microsoft's state-of-the-art LLM model Llama 2
o	Translation back into the user's native language through this model. 
	Many alternatives such as Huawei Text Translation have been tried before this stage
o	Measuring the health of the product with a metric between 0 and 100.
Services field Steps
o	RDS: Here we stored user information through MySQL service.
o	OBS: We have stored the photo we used through this service.
o	SWR: Here we had the opportunity to store our Docker images.
o	CC: Through this service, we created a Cluster environment and enabled our applications to run here.
o	EIP: We connected private IPs to public IPs through this service. 
Innovatıons
o	Analyzing how compatible the product is with the person from the photo of a product that is not in the user's native language and expressing this clearly to the user.
o	To make it easier for people with visual impairment to see the content of the products with the help of the light of their phone using the application we made. 
Business Value
CodeCheck is an app that helps you visualize vital nutrition information in a simple and intuitive manner. You can scan barcodes or EAN numbers or enter products manually to get a full reading of the ingredients and determine whether they are vegan, vegetarian, or gluten-free. 
    + Availability of mobile application
    - Inability to analyze the content of products
EWG's Healthy Living is an app that helps you understand your food, personal care, and other household products better. You can scan a barcode or search product names to quickly identify and analyze ingredients and see their overall rating, with 1.0 being the best and 10.0 being the worst in terms of risk and toxicity. 
                    + Mobile applications are available
                     - Product content cannot be analyzed
Conclusion
o	As a result of this project, visually impaired people will be able to learn the content of a product comfortably.
o	At the same time, people traveling abroad will be able to get detailed information about a product in a language they do not understand using our application. 

o	In short, our overall goal is to create conscious consumers.
You can access the source code from this link below.
https://github.com/ergundenizbuyruk/Huawei-Btk-Arge-Kodlama-Maratonu
Project Manager: Alperen ÇELİK
Researcher: Onur MALCI
Developer: Ergün Deniz BUYRUK


