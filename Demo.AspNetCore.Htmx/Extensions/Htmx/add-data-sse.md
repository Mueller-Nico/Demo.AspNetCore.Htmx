## The `add-data-sse` extension

Adds the data from an incoming sse event to a triggered request.

### Usage

hx-vals attributes are created from the SSE Data.  
If the sse-event **data** is a **JSON string**, it's included in 'hx-vals' as it is.

Which values are included in the request can be controlled with hx-params.  
For details check out the hx-vals and hx-params documentation.

### Sample

```sse
type: evtcust
data: {"mode":"new_customer","customerId":"4de0e2b5","orderId": 2245679}
```

results in  

```html
hx-vals='{"mode":"new_customer","customerId":"4de0e2b5","orderId": 2245679}'  
```

**Note:**

The *type* of the sse event matches the *value* of *hx-trigger*, in this case **evtcust**.  
For more information see the **server-sent-events Extension** documentation: <https://htmx.org/extensions/server-sent-events/>

```html
<div hx-ext="add-data-sse">
  <div hx-ext="sse" sse-connect="/sse-notifications">
      <div hx-get="/customer/details"
           hx-trigger="sse:evtcust"
           hx-swap="afterbegin"
           hx-target="#idCustomerTableBody"
           hx-params="mode,customerId">
      </div>

      <!--
        resulting url: 
            https://yourwebsite/customer/details?mode=new_customer&customerId=4de0e2b5
        
         In the absence of hx-params everything is send. 
         resulting url: 
            https://yourwebsite/customer/details?mode=new_customer&customerId=4de0e2b5&orderId=2245679
      --> 

   <!--and/or--> 
      <div hx-get="/order/details" 
           hx-trigger="sse:evtcust"
           hx-swap="beforeend"
           hx-target="#idOrderTable"
           hx-params="orderId">
      </div>
       <!--
         resulting url: 
            https://yourwebsite/order/details?orderId=2245679
      --> 
    </div>
  </div>
</div>
```

If the sse-event **data** is NOT a **JSON string**, it's included in 'hx-vals' as value. Default key is **data**

```sse
type: evtcust
data: "4de0e2b5"
```

results in:

```json
hx-vals='{"data":"4de0e2b5"}'  
```

```html
<div hx-ext="add-data-sse">
  <div hx-ext="sse" sse-connect="/sse-notifications">

       <!-- It is possible to set the param name. --> 
      <div hx-get="/customer/details"
           hx-trigger="sse:evtcust"
           hx-swap="afterbegin"
           hx-target="#idCustomerTableBody"
           hx-params="id">
      </div> 
      <!--
        resulting url: 
            https://yourwebsite/customer/details?id=4de0e2b5
      --> 

       <!--
        hx-params="*" or hx-params not specified
      --> 
      <div hx-get="/customer/details"
           hx-trigger="sse:evtcust"
           hx-swap="afterbegin"
           hx-target="#idCustomerTableBody">
      </div>
      <!--
        resulting url: 
            https://yourwebsite/customer/details?data=4de0e2b5
      --> 
    </div>
  </div>
</div>
```

### Notes and limitations

- If the sse-event **data** is not a **JSON string** and hx-params is used, the hx-params value must not start with a digit and may only contain word characters.  
- If the **hx-vals** attribute already exists, it will be overwritten.  

### Source
