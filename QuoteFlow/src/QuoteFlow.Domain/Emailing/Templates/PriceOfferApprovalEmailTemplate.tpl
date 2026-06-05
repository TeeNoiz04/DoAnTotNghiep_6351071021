
{{ if model.price_offer.status == "REJECTED" }}
    <p>Dear Mr/Ms {{ model.price_offer.creator_name }},</p>
    <p>The price offer has been rejected.</p>

{{ else if model.price_offer.status == "APPROVED" && model.approved_not_confirm != "" }}
    <p>Dear Mr/Ms {{ model.price_offer.creator_name }},</p>
    <p>The price offer has been approved

{{ else if model.price_offer.status == "APPROVED" && model.price_offer.result_status == "PENDING"  }}
    <p>Dear Sale PIC,</p>
    <p>The price offer has been approved. Please confirm Win or Lost.</p>

{{ else if model.price_offer.status == "APPROVED" && model.price_offer.result_status == "WON"   }}
    <p>Dear Mr/Ms {{ model.price_offer.creator_name }},</p>
    <p>The price offer has been confirmed as <strong>Win</strong>.</p>

{{ else if model.price_offer.status == "APPROVED" && model.price_offer.result_status == "LOST"  }}
    <p>Dear Mr/Ms {{ model.price_offer.creator_name }},</p>
    <p>The price offer has been confirmed as <strong>Lost</strong>.</p>

{{ else if model.price_offer.status == "APPROVED" && model.price_offer.result_status == "PRE_ORDER" }}
    <p>Dear Mr/Ms {{ model.price_offer.creator_name }},</p>
    <p>The price offer has been marked as <strong>Pre-order</strong>. Please confirm Win or Lost.</p>

{{ else if model.action == "SpecialInputPriceAssigned"}}
    <p>Dear Mr/Ms {{ model.price_offer.creator_name }},</p>
    <p>The price offer has been applied with a special input price.</p>
   
{{ else }}
    <p>Dear Approver(s),</p>
    <p>A new price offer is waiting for your action.</p>
{{ end }}



<h4>Price Offer Detail</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 30%">Price Offer Code</td>
            <td style="width: 70%">{{ model.price_offer.price_offer_code }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Project Name</td>
            <td style="width: 70%">{{ model.price_offer.project_name }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Buyer Name</td>
            <td style="width: 70%">{{ model.price_offer.buyer_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Material Type</td>
            <td>{{ model.price_offer.material_type }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Total Standard</td>
            <td style="width: 70%">{{ model.price_offer.total_standard  | math.format "N0"}}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Total Offer</td>
            <td style="width: 70%">{{ model.price_offer.total_offer | math.format "N0" }}</td>
        </tr>
    </table>
</div>
{{ if model.special_input_price_id != "" }}
    <h4>Special Input Price</h4>
    <div class="table-container">
        <table border="1">
            <tr>
                <td class="text-bold" style="width: 30%">Account No</td>
                <td style="width: 70%">{{ model.account_no }}</td>
            </tr>
            <tr>
                <td class="text-bold" style="width: 30%">Customer Name</td>
                <td style="width: 70%">{{ model.customer_name }}</td>
            </tr>
            <tr>
                <td class="text-bold" style="width: 30%">Project Name</td>
                <td style="width: 70%">{{ model.project_name }}</td>
            </tr>
            

            
        </table>
    </div>
{{ end }}

<h4>Approval History:</h4>
<div class="table-container">
    <table border="1">
        <tr style="background-color: lightgrey;">
            <th>Name</th>
            <th>Role</th>
            <th>Action</th>
            <th>Date</th>
            <th>Comment</th>
        </tr>
        
        {{ for history in model.approval_histories }}
        <tr>
            <td>{{ history.approver_full_name }}</td>
            <td>{{ history.approver_role_name }}</td>
            <td>{{ history.action }}</td>
            <td>{{ history.action_date }}</td>
            <td>{{ history.note }}</td>
        </tr>
        {{ end }}
    </table>
</div>

<p>Click <a href="{{ model.approval_route }}">here</a> to take action.</p>

