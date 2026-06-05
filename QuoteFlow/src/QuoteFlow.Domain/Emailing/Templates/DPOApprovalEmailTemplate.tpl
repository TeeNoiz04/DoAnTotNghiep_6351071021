{{ if model.action == "Rejected" }}
    <p>Dear Mr/Ms {{ model.dpo.creator_name }},</p>
    <p>The distributor purchase order has been rejected.</p>
{{ else if model.action == "Confirmed" }}
    <p>Dear Mr/Ms {{ model.dpo.creator_name }},</p>
    <p>The distributor purchase order has been confirmed.</p>
{{ end }}

<h4>DPO Information</h4>
<div class="table-container">
    <table border="1">
        <tr>
            <td class="text-bold" style="width: 30%">DPO No</td>
            <td style="width: 70%">{{ model.dpo.no }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Buyer Name</td>
            <td style="width: 70%">{{ model.dpo.buyer_name }}</td>
        </tr>
        <tr>
            <td class="text-bold">Confirm Date</td>
            <td>{{ model.dpo.confirm_date }}</td>
        </tr>
        <tr>
            <td class="text-bold">Material Type</td>
            <td>{{ model.dpo.material_type }}</td>
        </tr>
         <tr>
            <td class="text-bold">Note</td>
            <td>{{ model.dpo.note }}</td>
        </tr>
        <tr>
            <td class="text-bold" style="width: 30%">Total Amount</td>
            <td style="width: 70%">{{ model.dpo.total_amount | math.format "N0" }}</td>
        </tr>
        
    </table>
</div>
