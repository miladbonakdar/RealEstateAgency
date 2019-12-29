<template>
  <AppHeaderDropdown right no-caret>
    <template slot="header">
      <img
        src="/img/avatars/6.jpg"
        class="img-avatar"
        alt="admin@bootstrapmaster.com"/>
    </template>
    <template slot="dropdown">
      <!-- <b-dropdown-header tag="div" class="text-center"><strong>Account</strong></b-dropdown-header>
      <b-dropdown-item><i class="fa fa-bell-o"/> Updates
        <b-badge variant="info">{{ itemsCount }}</b-badge>
      </b-dropdown-item>
      <b-dropdown-item><i class="fa fa-envelope-o"/> Messages
        <b-badge variant="success">{{ itemsCount }}</b-badge>
      </b-dropdown-item>
      <b-dropdown-item><i class="fa fa-tasks"/> Tasks
        <b-badge variant="danger">{{ itemsCount }}</b-badge>
      </b-dropdown-item>
      <b-dropdown-item><i class="fa fa-comments"/> Comments
        <b-badge variant="warning">{{ itemsCount }}</b-badge>
      </b-dropdown-item>
      <b-dropdown-header
        tag="div"
        class="text-center">
        <strong>Settings</strong>
      </b-dropdown-header> -->
      <!-- <b-dropdown-item @click="gotoProfile"><i class="fa fa-user"/> Profile</b-dropdown-item>
      <b-dropdown-item><i class="fa fa-wrench"/> Settings</b-dropdown-item>
      <b-dropdown-item><i class="fa fa-usd"/> Payments
        <b-badge variant="secondary">{{ itemsCount }}</b-badge>
      </b-dropdown-item> -->
      <b-dropdown-item v-on:click="gotoProjects"><i class="fa fa-file"/> Projects
        <!-- <b-badge variant="primary">{{ itemsCount }}</b-badge> -->
      </b-dropdown-item>
      <b-dropdown-divider/>
      <!-- <b-dropdown-item><i class="fa fa-shield"/> Lock Account</b-dropdown-item> -->
      <b-dropdown-item v-on:click="logOut"><i class="fa fa-lock"/> Logout</b-dropdown-item>
    </template>
  </AppHeaderDropdown>
</template>

<script>
    import {HeaderDropdown as AppHeaderDropdown} from '@coreui/vue'

    export default {
        name: 'DefaultHeaderDropdownAccnt',
        components: {
            AppHeaderDropdown
        },
        data: () => {
            return {itemsCount: 42}
        },
        methods: {
            gotoProfile() {
                this.$router.pushLang('/profile')
            },
            logOut() {
               this.$toasted.show('Are you sure you want to logout?', {
                    action: [
                        {
                            text: 'Yes',
                            onClick: (e, toastObject) => {
                                localStorage.removeItem("user");
                localStorage.removeItem("loggedOn");
                localStorage.removeItem("token");
                document.location.href = `/${this.$route.params.lang}/auth/login`;
                                toastObject.goAway(0);
                            },
                        },
                        {
                            text: 'No',
                            onClick: (e, toastObject) => {
                                toastObject.goAway(0);
                            },
                        }
                    ]
                });


            },
            gotoProjects() {
                this.$router.replaceLang('/agent/requests');

            }
        }
    }
</script>
