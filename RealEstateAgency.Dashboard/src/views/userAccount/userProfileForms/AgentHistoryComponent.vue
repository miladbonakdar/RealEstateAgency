<template>
  <b-row>
    <b-col sm="6" md="6" lg="6">
      <b-card header="Agent Actions">
        <b-card-group deck>
            <div class="col-md-12 col-sm-12 col-lg-12 my-2">
              <b-card
                header-bg-variant="warning"
                border-variant="warning"
                header="Warning"
                align="center"
                header-tag="header"
                footer-tag="footer"
              >
                <h6 slot="header" class="mb-0">Agent 1</h6>
                <b-card-text>
                  Action Type: Call
                  <br />Date Time : 2019/02/12 08:36:54
                  <br />Summary: This is test summary.
                  <br />Status: Success
                  <br />Added at 2019/12/02
                </b-card-text>
              </b-card>
            </div>
        </b-card-group>
      </b-card>
    </b-col>
    <b-col sm="6" md="6" lg="6">
      <b-card header="Agent Reminders">
        <b-card-group deck>
            <div class="col-md-12 col-sm-12 col-lg-12 my-2">
              <b-card
                header-bg-variant="danger"
                border-variant="danger"
                header="danger"
                align="center"
                header-tag="header"
                footer-tag="footer"
              >
                <h6 slot="header" class="mb-0">Agent 1</h6>
                <b-card-text>
                  Action Type: 
                  <br />Date Time : 2019/02/12 08:36:54
                  <br />Summary: This is test summary.
                  <br />Added at 2019/12/02
                </b-card-text>
              </b-card>
            </div>
        </b-card-group>
      </b-card>
    </b-col>
  </b-row>
</template>

<script>
export default {};
</script>

<style>
</style>
