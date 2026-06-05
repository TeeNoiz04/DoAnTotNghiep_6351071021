<!DOCTYPE html>
<html dir="ltr" lang="en">
  <head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Mitsubishi Electric Communication</title>
    <style type="text/css">
      /* Reset and Base Styles */
      body,
      table,
      td,
      a {
        -webkit-text-size-adjust: 100%;
        -ms-text-size-adjust: 100%;
      }

      .text-bold {
        font-weight: bold;
      }

      table,
      td {
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
      }

      img {
        -ms-interpolation-mode: bicubic;
        border: 0;
        height: auto;
        line-height: 100%;
        outline: none;
        text-decoration: none;
      }

      /* Professional Table Styling */
      table {
        border-collapse: collapse;
        width: 100%;
        margin-bottom: 5px;
      }

      table th,
      table td {
        padding: 3px;
        border: 1px solid #e0e0e0;
        text-align: left;
        word-wrap: break-word;
        white-space: pre-wrap;
      }

      table th {
        font-weight: 600;
        background-color: #f5f5f5;
        border-bottom: 2px solid #d0d0d0;
      }

      /* Remove colored row styling since user doesn't like colors */
      table tr:nth-child(even) {
        background-color: #f4f4f4;
      }

      /* Add this to your existing <style> section */
      .table-container {
        width: 100%;
        max-width: 700px; /* Set your desired maximum width here */
        margin-right: auto;
        margin-left: 0; /* This ensures left alignment */
        overflow-x: auto;
      }

      table {
        width: 100%;
        max-width: 100%;
        table-layout: fixed;
      }

      /* Responsive Typography */
      @media screen and (max-width: 600px) {
        .responsive-text {
          font-size: 14px !important;
          line-height: 100% !important;
        }

        .mobile-center {
          text-align: center !important;
        }

        .mobile-full-width {
          width: 100% !important;
          max-width: 100% !important;
        }

        /* Table responsiveness with horizontal scrolling */
        .table-container {
          width: 100%;
          max-width: 500px;
          overflow-x: auto;
          -webkit-overflow-scrolling: touch;
        }

        /* Keep padding consistent but slightly reduced on mobile */
        table th,
        table td {
          padding: 3px !important;
          font-size: 13px !important;
        }
      }

      /* Email Client Fixes */
      #outlook a {
        padding: 0;
      }

      .ExternalClass {
        width: 100%;
      }

      .ExternalClass,
      .ExternalClass p,
      .ExternalClass span,
      .ExternalClass font,
      .ExternalClass td,
      .ExternalClass div {
        line-height: 100%;
      }
    </style>
  </head>
  <body style="margin: 0; padding: 0; width: 100%; background-color: #fafafa">
    <div role="article" lang="en" style="background-color: #fafafa">

      <!-- Dynamic Content Placeholder -->
      {{ content }}

      <p>Best regards,</p>
      <i>**This is an automatic email, please don't reply**</i>
    </div>
  </body>
</html>
