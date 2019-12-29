<template>
  <b-col sm="12">
    <b-card>
      <div slot="header">
        <strong>Edit Account Information</strong>
      </div>
      <b-row>
        <b-img
          class="userPicture"
          src="https://picsum.photos/125/125/?image=58"
          rounded="circle"
          center
          alt="Circle image"
        ></b-img>
      </b-row>
      <b-row>
        <b-col sm="6">
          <b-form-group>
            <label>First Name</label>
            <b-form-input v-model="userInfo.firstName" type="text" id="city" placeholder="Enter your fist name"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Last Name</label>
            <b-form-input v-model="userInfo.lastName" type="text" id="postal-code" placeholder="Enter your last name"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Middle Name</label>
            <b-form-input v-model="userInfo.middleName" type="text" id="postal-code" placeholder="Enter your middle name"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">User Name</label>
            <b-form-input v-model="userInfo.userName" type="text" id="postal-code" placeholder="Enter your user name"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Email</label>
            <b-form-input v-model="userInfo.email" type="text" id="postal-code" placeholder="Enter your email"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Phone number 1</label>
            <b-form-input v-model="userInfo.phone01" type="text" id="postal-code" placeholder="Enter your phone number 1"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Phone number 2</label>
            <b-form-input v-model="userInfo.phone02" type="text" id="postal-code" placeholder="Enter your phone number 2"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Address 1</label>
            <b-form-input v-model="userInfo.address01" type="text" id="postal-code" placeholder="Enter your first address"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Address 2</label>
            <b-form-input v-model="userInfo.address02" type="text" id="postal-code" placeholder="Enter your second address"></b-form-input>
          </b-form-group>
        </b-col>
        <b-col sm="6">
          <b-form-group>
            <label for="postal-code">Zip Code</label>
            <b-form-input v-model="userInfo.zipCode" type="text" id="postal-code" placeholder="Enter your Zip Code"></b-form-input>
          </b-form-group>
        </b-col>
      </b-row>
      <b-row>
        <submit-group v-on:onCancel="onCancel" v-on:onSubmit="onSubmit" />
      </b-row>
    </b-card>
  </b-col>
</template>

<script>
export default {
  name: "InformationForm",
  data() {
    return {
      userInfo: {
        firstName: "marjan",
        lastName: "ahmadi",
        middleName: "marj",
        userName: "marjaan_ahmdi",
        email: "marjanahmadi.1994@yahoo.com",
        phone01: "09120363527",
        phone02: "",
        address01: "Tehran, Sattarkhan",
        address02: "",
        zipCode: "12345678",
        userPicture: ""
      },
      selected: [], // Must be an array reference!
      show: true,
    };
  },
  methods: {
    onSubmit() {
      // do nothing
    },
    onCancel() {}
  }
};
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
} 
.userPicture{
    margin-bottom: 10px
}
</style>
