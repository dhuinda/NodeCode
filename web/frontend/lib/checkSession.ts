const checkSession = async () => {
  try {
    await fetch('/api/v1/users/user')
  } catch {
    window.location.href = '/api/v1/oauth2/code/github'
  }
}

export default checkSession
