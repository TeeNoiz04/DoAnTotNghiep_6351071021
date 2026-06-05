<h3>New Asset Lending Request</h3>

<p>Dear Asset Owners,</p>

<p>
A new lending request has been submitted for the asset(s) you are responsible for.
</p>

<h4>Request Information</h4>

<div class="table-container">
    <table border="1" cellpadding="6" cellspacing="0" style="border-collapse: collapse;">
        <tr>
            <td class="text-bold" style="width: 30%">Request No</td>
            <td style="width: 70%">{{model.request_no}}</td>
        </tr>
        <tr>
            <td class="text-bold">Requester</td>
            <td>{{model.requester_full_name}}</td>
        </tr>
        <tr>
            <td class="text-bold">Submitted Date</td>
            <td>{{model.formatted_submitted_date}}</td>
        </tr>
         <tr>
            <td class="text-bold">Request Title</td>
            <td>{{model.request_title}}</td>
        </tr>
        <tr>
            <td class="text-bold">Note</td>
            <td>{{model.note}}</td>
        </tr>
    </table>
</div>

<p>
Please review and take action if necessary. You can view the full request details by clicking 
<a  href="{{model.approval_url}}">here</a>.
</p>
