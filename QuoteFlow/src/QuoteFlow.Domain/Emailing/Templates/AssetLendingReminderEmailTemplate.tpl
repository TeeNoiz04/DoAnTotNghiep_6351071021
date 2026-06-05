<p>Dear Mr/Ms {{model.receiver_full_name}},</p>

<p>This is a friendly reminder that the item(s) you have borrowed under the asset lending request below are currently overdue for return. Please arrange to return them as soon as possible.</p>
<h4>Request Information</h4>
<div class="table-container">
    <table border="1">
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
            <td class="text-bold">Due Date</td>
            <td>{{model.due_date}}</td>
        </tr>
    </table>
</div>


<p>Please return the borrowed item(s) at your earliest convenience. You can view the full request details by clicking <a href="{{ model.approval_url }}">here</a>.</p>